using ApiApp.Models;
using ApiApp.Services.ReportsService;
using InterWMSDesctop.Services.DialogService;
using LiveCharts;
using LiveCharts.Configurations;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ApiApp.Extensions;
using InterWMSDesctop.Views;
using InterWMSDesctop.Services;

namespace InterWMSDesctop.ViewModels
{
    class ReportsVM : ViewModelBase
    {
        #region Fields
        private readonly IReportsService _reportsService;
        private readonly IDialogService _dialogService;
        private DateTime _startSaleDate = DateTime.Now.AddDays(-7);
        private DateTime _endSaleDate = DateTime.Now;
        private DateTime _startReportDate = DateTime.Now.AddDays(-7);
        private DateTime _endReportDate = DateTime.Now;
        private string _title;
        private OperationType _type;
        private ProductsReport _reportData;
        #endregion

        #region Constructor
        public ReportsVM(IReportsService reportsService,
                         IDialogService dialogService)
        {
            _reportsService = reportsService;
            _dialogService = dialogService;
        }
        #endregion

        #region Properties
        public DateTime StartSaleDate
        {
            get => _startSaleDate;
            set => OnPropertyChanged(ref _startSaleDate, value, () => StartSaleDate);
        }

        public DateTime EndSaleDate
        {
            get => _endSaleDate;
            set => OnPropertyChanged(ref _endSaleDate, value, () => EndSaleDate);
        }

        public DateTime StartPurchaseDate
        {
            get => _startReportDate;
            set => OnPropertyChanged(ref _startReportDate, value, () => StartPurchaseDate);
        }

        public DateTime EndPurchaseDate
        {
            get => _endReportDate;
            set => OnPropertyChanged(ref _endReportDate, value, () => EndPurchaseDate);
        }

        public string Title => _title;

        public ChartValues<ReportProduct> Results { get; private set; }
        public ObservableCollection<string> Labels { get; private set; }
        public object Mapper { get; set; }
        #endregion

        #region Commands
        #region CreateSaleReportCmd
        private ICommand _createSaleReportCmd;

        public ICommand CreateSaleReportCmd
            => _createSaleReportCmd ?? (_createSaleReportCmd = new AsyncCommand(CreateSaleReport, CanCreateSaleReport));

        private bool CanCreateSaleReport(object obj)
        {
            return StartSaleDate != null && EndSaleDate != null && StartSaleDate < EndSaleDate;
        }

        private async Task CreateSaleReport(object obj)
        {
            try
            {
                if (!CanCreateSaleReport(obj))
                {
                    return;
                }

                var reportData = await _reportsService.GetSalesProductsReport(StartSaleDate, EndSaleDate);

                if (reportData != null)
                {
                    _type = OperationType.Shipping;
                    FillReportDate(reportData);
                }
            }
            catch (Exception ex)
            {
                await _dialogService.ShowErrorDialog(ex.Message);
            }
        }
        #endregion
        #region CreatePurchaseReportCmd
        private ICommand _createPurchaseReportCmd;

        public ICommand CreatePurchaseReportCmd
            => _createPurchaseReportCmd ?? (_createPurchaseReportCmd = new AsyncCommand(CreatePurchaseReport, CanCreatePurchaseReport));

        private bool CanCreatePurchaseReport(object obj)
        {
            return StartPurchaseDate != null && EndPurchaseDate != null && StartPurchaseDate < EndPurchaseDate;
        }

        private async Task CreatePurchaseReport(object obj)
        {
            try
            {
                if (!CanCreatePurchaseReport(obj))
                {
                    return;
                }


                var reportData = await _reportsService.GetPurchaseProductsReport(StartPurchaseDate, EndPurchaseDate);

                if (reportData != null)
                {
                    _type = OperationType.Reception;
                    FillReportDate(reportData);
                }
            }
            catch (Exception ex)
            {
                await _dialogService.ShowErrorDialog(ex.Message);
            }
        }
        #endregion

        #region FullReportProductCmd
        private ICommand _fullReportProductCmd;

        public ICommand FullReportProductCmd
            => _fullReportProductCmd ?? (_fullReportProductCmd = new RelayCommand(FullReportProduct, CanFullReportProduct));

        private bool CanFullReportProduct(object obj)
        {
            return obj is ChartPoint chartPoint && chartPoint.Instance is ReportProduct;
        }

        private void FullReportProduct(object obj)
        {
            if (!CanFullReportProduct(obj))
            {
                return;
            }

            var reportProduct = (obj as ChartPoint).Instance as ReportProduct;
            var title = _type == OperationType.Reception ? $"Подробный отчет по приему {reportProduct.Name}" : $"Подробный отчет по продаже {reportProduct.Name}";
            var infos = new ProductInfosReportVM(reportProduct, title);
            var view = new ProductInfosReportV
            {
                DataContext = infos
            };

            view.ShowDialog();
        }
        #endregion

        #region SaveExcelCmd
        private ICommand _saveExcelCmd;

        public ICommand SaveExcelCmd
            => _saveExcelCmd ?? (_saveExcelCmd = new AsyncCommand(SaveExcel));

        private async Task SaveExcel(object obj)
        {
            try
            {
                var excel = new ExcelTemplateService();
                excel.SaveReport(_reportData);
            }
            catch (Exception ex)
            {
                await _dialogService.ShowErrorDialog(ex.Message);
            }
        }
        #endregion
        #endregion

        #region Private methods
        private void FillReportDate(ProductsReport reportData)
        {
            _reportData = reportData;
            _title = reportData.ReportName;
            Mapper = Mappers.Xy<ReportProduct>().X((p, index) => index).Y(p => p.Count);
            Results = new ChartValues<ReportProduct>(reportData.ReportProducts);
            Labels = new ObservableCollection<string>(reportData.ReportProducts.Select(w => w.Name));

            OnPropertyChanged(() => Title);
            OnPropertyChanged(() => Mapper);
            OnPropertyChanged(() => Results);
            OnPropertyChanged(() => Labels);
            CommandManager.InvalidateRequerySuggested();
        }
        #endregion
    }
}
