using System.Threading.Tasks;
using WpfAppDesktopUI.Library.Models;

namespace WpfAppDesktopUI.Library.Api
{
    public interface ISaleEndpoint
    {
        Task PostSale(SaleModel sale);
    }
}