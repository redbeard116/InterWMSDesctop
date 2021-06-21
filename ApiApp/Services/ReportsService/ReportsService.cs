using ApiApp.Extensions;
using ApiApp.Models;
using ApiApp.Providers.RequestProvider;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApiApp.Services.ReportsService
{
    public class ReportsService : IReportsService
    {
        #region Fields
        private readonly ILogger<ReportsService> _logger;
        private readonly IRequestProvider _requestProvider;
        #endregion

        #region Constructor
        public ReportsService(ILogger<ReportsService> logger,
                              IRequestProvider requestProvider)
        {
            _logger = logger;
            _requestProvider = requestProvider;
        }
        #endregion

        #region IReportsService
        public async Task<ProductsReport> GetPurchaseProductsReport(DateTime startTime, DateTime endTime)
        {
            try
            {
                _logger.LogDebug("GetPurchaseProductsReport");
                var start = startTime.GetUnixTime().ToString();
                var end = endTime.GetUnixTime().ToString();

                return await _requestProvider.GetJson<ProductsReport>($"api/Reports/purchase?startTime={start}&endTime={end}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(GetPurchaseProductsReport)}");
                throw;
            }
        }

        public async Task<ProductsReport> GetSalesProductsReport(DateTime startTime, DateTime endTime)
        {
            try
            {
                _logger.LogDebug("GetSalesProductsReport");
                var start = startTime.GetUnixTime().ToString();
                var end = endTime.GetUnixTime().ToString();

                return await _requestProvider.GetJson<ProductsReport>($"api/Reports/sales?startTime={start}&endTime={end}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(GetSalesProductsReport)}");
                throw;
            }
        }
        #endregion
    }
}
