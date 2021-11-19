using System.Collections.Generic;
using System.Threading.Tasks;
using WpfAppDesktopUI.Library.Models;

namespace WpfAppDesktopUI.Library.Api
{
    public interface IUserEndpoint
    {
        Task<List<UserModel>> GetAll();
    }
}