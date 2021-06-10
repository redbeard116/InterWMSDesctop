using ApiApp.Models;
using ApiApp.Providers.RequestProvider;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiApp.Services.CounterpartyService
{
    public class CounterpartyService : ICounterpartyService
    {
        #region Fields
        private readonly ILogger<CounterpartyService> _logger;
        private readonly IRequestProvider _requestProvider;
        #endregion

        #region Constructor
        public CounterpartyService(ILogger<CounterpartyService> logger,
                                   IRequestProvider requestProvider)
        {
            _logger = logger;
            _requestProvider = requestProvider;
        }
        #endregion

        #region ICounterpartyService
        public async Task<Counterparty> AddCounterparty(Counterparty counterparty)
        {
            try
            {
                _logger.LogInformation("Add new counterparty");
                return await _requestProvider.PostJson<Counterparty>("api/Counterpartyes", counterparty.ToJson());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(AddCounterparty)}");
                throw;
            }
        }

        public async Task<bool> DeleteCounterparty(int id)
        {
            try
            {
                _logger.LogInformation($"Delete counterparty {id}");
                await _requestProvider.Delete($"api/Counterpartyes/{id}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(DeleteCounterparty)}");
                throw;
            }
        }

        public async Task<Counterparty> EditCounterparty(Counterparty counterparty)
        {
            try
            {
                _logger.LogInformation($"Edit ccounterparty {counterparty.Id}");
                return await _requestProvider.PutJson<Counterparty>($"api/Counterpartyes/edit/{counterparty.Id}", counterparty.ToJson());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(EditCounterparty)}");
                throw;
            }
        }

        public async Task<Counterparty> GetCounterparty(int id)
        {
            try
            {
                _logger.LogInformation($"Get counterparty {id}");
                return await _requestProvider.GetJson<Counterparty>($"api/Counterpartyes/{id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(GetCounterparty)}");
                throw;
            }
        }

        public async Task<IEnumerable<Counterparty>> GetCounterpartyes()
        {
            try
            {
                _logger.LogInformation("Get ounterpartyes");
                var result = await _requestProvider.GetJsonString("api/Counterpartyes");
                var json = JsonConvert.DeserializeObject<IEnumerable<Counterparty>>(result);
                return json;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(GetCounterpartyes)}");
                throw;
            }
        }
        #endregion
    }
}
