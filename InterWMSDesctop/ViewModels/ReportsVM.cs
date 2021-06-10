using ApiApp.Services.ReportsService;

namespace InterWMSDesctop.ViewModels
{
    class ReportsVM : ViewModelBase
    {
        #region Fields
        private readonly IReportsService _reportsService;
        #endregion

        #region Constructor
        public ReportsVM(IReportsService reportsService)
        {
            _reportsService = reportsService;
        }
        #endregion
    }
}
