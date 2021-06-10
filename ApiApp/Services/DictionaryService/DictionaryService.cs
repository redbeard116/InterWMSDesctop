using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiApp.Models;
using ApiApp.Providers.RequestProvider;
using Newtonsoft.Json;

namespace ApiApp.Services.DictionaryService
{
    public class DictionaryService : IDictionaryService
    {
        #region Fields
        private readonly ILogger<DictionaryService> _logger;
        private readonly IRequestProvider _requestProvider;
        #endregion

        #region Constructor
        public DictionaryService(ILogger<DictionaryService> logger,
                                 IRequestProvider requestProvider)
        {
            _logger = logger;
            _requestProvider = requestProvider;
        }

        #region IDictionaryService
        public async Task<AccessType> AddAccessType(AccessType accessType)
        {
            _logger.LogInformation("Add access type");
            try
            {
                var result = await _requestProvider.PostJson<AccessType>("api/Dictionaryes/accessytypes", accessType.ToJson());
                return result;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex,$"Error in {nameof(AddAccessType)}");
                return null;
            }
        }

        public async Task<ProductType> AddProductTypes(ProductType productType)
        {
            _logger.LogDebug("Add product type");
            try
            {
                var result = await _requestProvider.PostJson<ProductType>("api/Dictionaryes/producttypes", productType.ToJson());
                return result;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(AddProductTypes)}");
                return null;
            }
        }

        public async Task<bool> DeleteAccessType(int id)
        {
            _logger.LogDebug($"Delete access type: {id}");
            try
            {
                await _requestProvider.Delete($"api/Dictionaryes/accessytypes/{id}");
                return true;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(DeleteAccessType)}");
                return false;
            }
        }

        public async Task<bool> DeleteProductTypes(int id)
        {
            _logger.LogDebug($"Delete product type: {id}");
            try
            {
                await _requestProvider.Delete($"api/Dictionaryes/producttypes/{id}");
                return true;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(DeleteProductTypes)}");
                return false;
            }
        }

        public async Task<IEnumerable<AccessType>> GetAccessTypes()
        {
            _logger.LogDebug("Get access types");
            try
            {
                var result = await _requestProvider.GetJsonString("api/Dictionaryes/accessytypes");
                var json = JsonConvert.DeserializeObject<IEnumerable<AccessType>>(result);
                return json;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(GetAccessTypes)}");
                return null;
            }
        }
        #endregion

        public async Task<IEnumerable<string>> GetOperationTypes()
        {
            _logger.LogDebug("Get operation types");
            try
            {
                var result = await _requestProvider.GetJsonString("api/Dictionaryes/operationtypes");
                var json = JsonConvert.DeserializeObject<IEnumerable<string>>(result);
                return json;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(GetOperationTypes)}");
                return null;
            }
        }

        public async Task<IEnumerable<ProductType>> GetProductTypes()
        {
            _logger.LogDebug("Get product types");
            try
            {
                var result = await _requestProvider.GetJsonString("api/dictionaryes/producttypes");
                var json = JsonConvert.DeserializeObject<IEnumerable<ProductType>>(result);
                return json;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(GetOperationTypes)}");
                return null;
            }
        }

        public async Task<IEnumerable<string>> GetRightsGrids()
        {
            _logger.LogDebug("Get right grids");
            try
            {
                var result = await _requestProvider.GetJsonString("api/Dictionaryes/rightgrids");
                var json = JsonConvert.DeserializeObject<IEnumerable<string>>(result);
                return json;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(GetRightsGrids)}");
                return null;
            }
        }

        public async Task<IEnumerable<UserRole>> GetUserRoles()
        {
            _logger.LogDebug("Get user roles");
            try
            {
                var result = await _requestProvider.GetJsonString("api/Dictionaryes/userroles");
                var json = JsonConvert.DeserializeObject<IEnumerable<UserRole>>(result);
                return json;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(GetUserRoles)}");
                return null;
            }
        }
        #endregion
    }
}
