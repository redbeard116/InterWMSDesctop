using ApiApp.Providers.RequestProvider;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
