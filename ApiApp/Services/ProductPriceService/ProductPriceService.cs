using ApiApp.Models;
using ApiApp.Providers.RequestProvider;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiApp.Services.ProductPriceService
{
    public class ProductPriceService : IProductPriceService
    {
        #region Fields
        private readonly ILogger<ProductPriceService> _logger;
        private readonly IRequestProvider _requestProvider;
        #endregion

        #region Constructor
        public ProductPriceService(ILogger<ProductPriceService> logger,
                                   IRequestProvider requestProvider)
        {
            _logger = logger;
            _requestProvider = requestProvider;
        }
        #endregion

        #region IOperationService
        public async Task<ProductPrice> AddProductPrice(ProductPrice productPrice)
        {
            try
            {
                _logger.LogInformation("Add product price");
                return await _requestProvider.PostJson<ProductPrice>("api/prices", productPrice.ToJson());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(AddProductPrice)}");
                throw;
            }
        }

        public async Task<bool> DeleteProductPrice(int id)
        {
            try
            {
                _logger.LogInformation($"Delete product price {id}");
                await _requestProvider.Delete($"api/Prices/{id}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(DeleteProductPrice)}");
                throw;
            }
        }

        public async Task<ProductPrice> EditProductPrice(ProductPrice productPrice)
        {
            try
            {
                _logger.LogInformation($"Edit product price {productPrice.Id}");
                return await _requestProvider.PutJson<ProductPrice>($"api/prices/edit/{productPrice.Id}", productPrice.ToJson());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(EditProductPrice)}");
                throw;
            }
        }

        public async Task<IEnumerable<ProductPrice>> GetLastPrices()
        {
            try
            {
                _logger.LogInformation("Get last product prices");
                var result = await _requestProvider.GetJsonString("api/prices/last");
                var json = JsonConvert.DeserializeObject<IEnumerable<ProductPrice>>(result);
                return json;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(GetLastPrices)}");
                throw;
            }
        }

        public async Task<ProductPrice> GetProductPrice(int id)
        {
            try
            {
                _logger.LogInformation($"Get product price {id}");
                return await _requestProvider.GetJson<ProductPrice>($"api/prices/{id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(GetProductPrice)}");
                throw;
            }
        }

        public async Task<IEnumerable<ProductPrice>> GetProductPrices()
        {
            try
            {
                _logger.LogInformation("Get product prices");
                var result = await _requestProvider.GetJsonString("api/prices");
                var json = JsonConvert.DeserializeObject<IEnumerable<ProductPrice>>(result);
                return json;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(GetProductPrices)}");
                throw;
            }
        }
        #endregion
    }
}
