using ApiApp.Models;
using ApiApp.Providers.RequestProvider;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiApp.Services.StorageAreaService
{
    public class StorageAreaService : IStorageAreaService
    {
        #region Fields
        private readonly ILogger<StorageAreaService> _logger;
        private readonly IRequestProvider _requestProvider;
        #endregion

        #region Constructor
        public StorageAreaService(ILogger<StorageAreaService> logger,
                                  IRequestProvider requestProvider)
        {
            _logger = logger;
            _requestProvider = requestProvider;
        }
        #endregion

        #region IStorageAreaService
        public async Task<StorageArea> AddStorageArea(StorageArea storageArea)
        {
            try
            {
                _logger.LogInformation("Add storage area");
                return await _requestProvider.PostJson<StorageArea>("api/StorageAreas", storageArea.ToJson());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(AddStorageArea)}");
                throw;
            }
        }

        public async Task<bool> DeleteStorageArea(int id)
        {
            try
            {
                _logger.LogInformation($"Delete storage area {id}");
                await _requestProvider.Delete($"api/StorageAreas/{id}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(DeleteStorageArea)}");
                throw;
            }
        }

        public async Task<StorageArea> EditStorageArea(StorageArea storageArea)
        {
            try
            {
                _logger.LogInformation($"Edit product storage {storageArea.Id}");
                return await _requestProvider.PutJson<StorageArea>($"api/StorageAreas/edit/{storageArea.Id}", storageArea.ToJson());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(EditStorageArea)}");
                throw;
            }
        }

        public async Task<IEnumerable<StorageArea>> GetStorageAreas()
        {
            try
            {
                _logger.LogInformation("Get storage area");
                var result = await _requestProvider.GetJsonString("api/StorageAreas");
                var json = JsonConvert.DeserializeObject<IEnumerable<StorageArea>>(result);
                return json;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(GetStorageAreas)}");
                throw;
            }
        }

        public async Task<StorageArea> GetStorageArea(int id)
        {
            try
            {
                _logger.LogInformation($"Get storage area {id}");
                return await _requestProvider.GetJson<StorageArea>($"api/StorageAreas/{id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(GetStorageArea)}");
                throw;
            }
        }
        #endregion
    }
}