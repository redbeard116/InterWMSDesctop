using ApiApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiApp.Services.ProductPriceService
{
    public interface IProductPriceService
    {
        Task<IEnumerable<ProductPrice>> GetProductPrices();

        Task<ProductPrice> GetProductPrice(int id);

        Task<ProductPrice> AddProductPrice(ProductPrice productPrice);
        Task<bool> DeleteProductPrice(int id);
        Task<ProductPrice> EditProductPrice(ProductPrice productPrice);
        Task<IEnumerable<ProductPrice>> GetLastPrices();
    }
}
