using System.Net.Http;
using System.Threading.Tasks;
using WpfAppDesktopUI.Models;

namespace WpfAppDesktopUI.Library.Api
{
    public interface IAPIHelper
    {
        Task<AuthenticatedUser> Authenticate(string userName, string password);
        Task GetLoggedInUserInfo(string token);
        HttpClient ApiClient { get; }
    }
}