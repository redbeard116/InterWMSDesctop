using ApiApp.Models;
using ApiApp.Providers.RequestProvider;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiApp.Services.ProductService
{
    public class ProductService : IProductService
    {
        #region Fields
        private readonly ILogger<ProductService> _logger;
        private readonly IRequestProvider _requestProvider;
        #endregion

        #region Constructor
        public ProductService(ILogger<ProductService> logger,
                              IRequestProvider requestProvider)
        {
            _logger = logger;
            _requestProvider = requestProvider;
        }
        #endregion

        #region IProductServices
        public async Task<Product> GetProduct(int id)
        {
            try
            {
                _logger.LogInformation($"GetProduct {id}");
                return await _requestProvider.GetJson<Product>($"api/products");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(GetProduct)}");
                throw;
            }
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            try
            {
                _logger.LogInformation("GetProducts");
                var result = await _requestProvider.GetJsonString("api/products");
                var json = JsonConvert.DeserializeObject<IEnumerable<Product>>(result);
                return json;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(GetProducts)}");
                throw;
            }
        }

        public async Task<Product> AddProduct(Product product)
        {
            try
            {
                _logger.LogInformation("AddProduct");
                return await _requestProvider.PostJson<Product>("api/products", product.ToJson());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(AddProduct)}");
                throw;
            }
        }

        public async Task<bool> DeleteProduct(int id)
        {
            try
            {
                _logger.LogInformation($"DeleteProduct {id}");
                await _requestProvider.Delete($"api/products/{id}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(DeleteProduct)}");
                throw;
            }
        }

        public async Task<Product> EditProduct(Product product)
        {
            try
            {
                _logger.LogInformation($"EditProduct {product.Id}");
                return await _requestProvider.PutJson<Product>($"api/products/edit/{product.Id}", product.ToJson());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(EditProduct)}");
                throw;
            }
        }
        #endregion
    }
}
