using System.Collections.Generic;
using System.Threading.Tasks;
using WpfAppDesktopUI.Library.Models;

namespace WpfAppDesktopUI.Library.Api
{
    public interface IUserEndpoint
    {
        Task<List<UserModel>> GetAll();
        Task<Dictionary<string, string>> GetAllRoles();
        Task AddUserToRole(string userId, string roleName);
        Task RemoveUserFromRole(string userId, string roleName);
    }
}