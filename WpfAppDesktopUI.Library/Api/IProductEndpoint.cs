using System.Collections.Generic;
using System.Threading.Tasks;
using WpfAppDesktopUI.Library.Models;

namespace WpfAppDesktopUI.Library.Api
{
    public interface IProductEndpoint
    {
        Task<List<ProductModel>> GetAll();
    }
}