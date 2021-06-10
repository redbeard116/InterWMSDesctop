using ApiApp.Models;
using ApiApp.Providers.RequestProvider;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiApp.Services.OperationService
{
    public class OperationService : IOperationService
    {
        #region Fields
        private readonly ILogger<OperationService> _logger;
        private readonly IRequestProvider _requestProvider;
        #endregion

        #region Constructor
        public OperationService(ILogger<OperationService> logger,
                                IRequestProvider requestProvider)
        {
            _logger = logger;
            _requestProvider = requestProvider;
        }
        #endregion

        #region IOperationService
        public async Task<Operation> AddOperation(Operation operation)
        {
            try
            {
                _logger.LogInformation("Add new operation");
                return await _requestProvider.PostJson<Operation>("api/Operations", operation.ToJson());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(AddOperation)}");
                throw;
            }
        }

        public async Task<bool> DeleteOperation(int id)
        {
            try
            {
                _logger.LogInformation($"Delete operation {id}");
                await _requestProvider.Delete($"api/Operations/{id}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(DeleteOperation)}");
                throw;
            }
        }

        public async Task<Operation> EditOperation(Operation operation)
        {
            try
            {
                _logger.LogInformation($"Edit operation {operation.Id}");
                return await _requestProvider.PutJson<Operation>($"api/Operations/edit/{operation.Id}", operation.ToJson());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(EditOperation)}");
                throw;
            }
        }

        public async Task<Operation> GetOperation(int id)
        {
            try
            {
                _logger.LogInformation($"Get operation {id}");
                return await _requestProvider.GetJson<Operation>($"api/Operations/{id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(GetOperation)}");
                throw;
            }
        }

        public async Task<IEnumerable<Operation>> GetOperations()
        {
            try
            {
                _logger.LogInformation("Get operation");
                var result = await _requestProvider.GetJsonString("api/Operations");
                var json = JsonConvert.DeserializeObject<IEnumerable<Operation>>(result);
                return json;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(GetOperations)}");
                throw;
            }
        }
        #endregion
    }
}
