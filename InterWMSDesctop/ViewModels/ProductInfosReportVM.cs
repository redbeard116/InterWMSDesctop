using ApiApp.Models;
using LiveCharts;
using LiveCharts.Configurations;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace InterWMSDesctop.ViewModels
{
    class ProductInfosReportVM : ViewModelBase
    {
        #region Fields
        private string _title;
        #endregion

        #region Constructor
        public ProductInfosReportVM(ReportProduct reportProduct, string title)
        {
            FillData(reportProduct, title);
        }
        #endregion

        #region Properties
        public string Title => _title;
        public ChartValues<ReportProductInfo> Results { get; private set; }
        public ObservableCollection<string> Labels { get; private set; }
        public object MapperCount { get; set; }
        public object MapperPrice { get; set; }
        #endregion

        #region Commands
        #region CloseCmd

        private ICommand _closeCmd;

        public ICommand CloseCmd
            => _closeCmd ?? (_closeCmd = new RelayCommand(Close));

        private void Close(object obj)
        {

            if (obj is Window window)
            {
                window.Close();
            }
        }
        #endregion
        #endregion

        #region Private methods
        private void FillData(ReportProduct reportProduct, string title)
        {
            _title = title;
            Results = new ChartValues<ReportProductInfo>(reportProduct.Infos);
            Labels = new ObservableCollection<string>(reportProduct.Infos.Select(w => w.Date));
            MapperCount = Mappers.Xy<ReportProductInfo>().X((p, index) => index).Y(p => p.Count);
            MapperPrice = Mappers.Xy<ReportProductInfo>().X((p, index) => index).Y(p => p.Price);

            OnPropertyChanged(() => Title);
            OnPropertyChanged(() => MapperCount);
            OnPropertyChanged(() => Results);
            OnPropertyChanged(() => Labels);
            OnPropertyChanged(() => MapperPrice);
        }
        #endregion
    }
}