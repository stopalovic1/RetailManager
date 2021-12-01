using DataManagerApi.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DataManagerApi.Controllers
{
    public class TokenController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public TokenController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Route("/token")]
        [HttpPost]
        public async Task<IActionResult> Create(string username, string password, string grant_type)
        {
            if (await IsValidUsernameAndPassword(username, password))
            {
                return new ObjectResult(await GenerateToken(username));
            }
            else
            {
                return BadRequest();
            }


        }

        [Route("/reverseToken")]
        [HttpPost]
        public IActionResult Reverse(string token)
        {
            string secret = "MySuperDuperSecretKey";
            var key = Encoding.ASCII.GetBytes(secret);
            var handler = new JwtSecurityTokenHandler();
            var validations = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
            var claims = handler.ValidateToken(token, validations, out var tokenSecure);

            return (new ObjectResult(tokenSecure));



            //var c = claims.Claims.ToList();
            //Dictionary<string, dynamic> dic = new Dictionary<string, dynamic>();
           
            //foreach (var cc in c)
            //{
            //    var index = cc.Type.LastIndexOf('/') + 1;
            //    string k = cc.Type;
            //    if (index != -1)
            //    {
            //        k = cc.Type.Substring(index);
            //    }
            //    if (k == "role")
            //    {
            //        if(!dic.ContainsKey("Roles"))
            //        {
            //            dic.Add("Roles", new List<string>());
            //        }
            //        dic["Roles"].Add(cc.Value);
            //    }
            //    else
            //    {
            //        dic.Add(k, cc.Value);
            //    }

            //}
            //return new ObjectResult(dic);
        }


        private async Task<bool> IsValidUsernameAndPassword(string username, string password)
        {
            var user = await _userManager.FindByEmailAsync(username);
            return await _userManager.CheckPasswordAsync(user, password);
        }


        private async Task<dynamic> GenerateToken(string username)
        {
            var user = await _userManager.FindByEmailAsync(username);
            var roles = from ur in _context.UserRoles
                        from u in _context.Users
                        join r in _context.Roles on ur.RoleId equals r.Id
                        where ur.UserId == user.Id
                        select new { ur.UserId, ur.RoleId, r.Name };

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(JwtRegisteredClaimNames.Nbf,new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp,new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString())
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            var token = new JwtSecurityToken(
                new JwtHeader(
                    new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySuperDuperSecretKey")),
                        SecurityAlgorithms.HmacSha256)),
                new JwtPayload(claims));


            var output = new
            {
                Access_token = new JwtSecurityTokenHandler().WriteToken(token),
                UserName = username
            };

            return output;

        }




    }
}
