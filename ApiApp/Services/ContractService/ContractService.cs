using ApiApp.Models;
using ApiApp.Providers.RequestProvider;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiApp.Services.ContractService
{
    public class ContractService : IContractService
    {
        #region Fields
        private readonly ILogger<ContractService> _logger;
        private readonly IRequestProvider _requestProvider;
        #endregion

        #region Constructor
        public ContractService(ILogger<ContractService> logger,
                               IRequestProvider requestProvider)
        {
            _logger = logger;
            _requestProvider = requestProvider;
        }
        #endregion

        #region IContractService
        public async Task<Contract> AddContract(Contract contract)
        {
            try
            {
                _logger.LogInformation("Add new contract");
                return await _requestProvider.PostJson<Contract>("api/Contracts", contract.ToJson());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(AddContract)}");
                throw;
            }
        }

        public async Task<bool> DeleteContract(int id)
        {
            try
            {
                _logger.LogInformation($"Delete contract {id}");
                await _requestProvider.Delete($"api/Contracts/{id}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(DeleteContract)}");
                throw;
            }
        }

        public async Task<Contract> EditContract(Contract contract)
        {
            try
            {
                _logger.LogInformation($"Edit contract {contract.Id}");
                return await _requestProvider.PutJson<Contract>($"api/Contracts/edit/{contract.Id}", contract.ToJson());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(EditContract)}");
                throw;
            }
        }

        public async Task<Contract> GetContract(int id)
        {
            try
            {
                _logger.LogInformation($"Get contract {id}");
                return await _requestProvider.GetJson<Contract>($"api/Contracts/{id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(GetContract)}");
                throw;
            }
        }

        public async Task<IEnumerable<Contract>> GetContracts()
        {
            try
            {
                _logger.LogInformation("Get contracts");
                var result = await _requestProvider.GetJsonString("api/Contracts");
                var json = JsonConvert.DeserializeObject<IEnumerable<Contract>>(result);
                return json;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(GetContracts)}");
                throw;
            }
        }
        #endregion
    }
}
