using ApiApp.Models;
using ApiApp.Services.ProductPriceService;
using ApiApp.Services.ProductService;
using InterWMSDesctop.Services.DialogService;
using System;
using ApiApp.Extensions;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace InterWMSDesctop.ViewModels.Acts
{
    class ProductPriceActVM : ViewModelBase
    {
        #region Fields
        private readonly IProductPriceService _productPriceService;
        private readonly IProductService _productService;
        private readonly IDialogService _dialogService;

        private ProductPrice _productPrice;
        private bool _isEdit;
        private double _cost;
        private Product _selectedProduct;
        private DateTime _date;
        #endregion

        #region Constructor
        public ProductPriceActVM(IProductPriceService productPriceService,
                                 IProductService productService,
                                 IDialogService dialogService)
        {
            _productPriceService = productPriceService;
            _productService = productService;
            _dialogService = dialogService;
        }
        #endregion

        #region Properties
        public bool IsEdit => _isEdit;

        public double Cost
        {
            get => _cost;
            set => OnPropertyChanged(ref _cost, value, () => Cost);
        }

        public Product SelectedProduct
        {
            get => _selectedProduct;
            set => OnPropertyChanged(ref _selectedProduct, value, () => SelectedProduct);
        }

        public DateTime Date => _date;

        public ObservableCollection<Product> Products { get; set; }

        public string ButtonContent => IsEdit ? "Применить" : "Создать";
        #endregion

        #region Public methods
        public async Task Load(ProductPrice productPrice, bool isEdit)
        {
            _isEdit = isEdit;
            var products = await _productService.GetProducts();
            Products = new ObservableCollection<Product>(products);
            if (IsEdit)
            {
                _productPrice = productPrice;
                _date = _productPrice.Date.GetNormalTime();
                SelectedProduct = Products.FirstOrDefault(w => w.Id == _productPrice.ProductId);
            }
            else
            {
                _productPrice = new ProductPrice();
                _date = DateTime.Now;
            }


            OnPropertyChanged(() => Date);
            OnPropertyChanged(() => IsEdit);
        }
        #endregion

        #region Commands
        #region Commands
        #region ProductPriceActCmd

        private ICommand _productPriceActCmd;

        public ICommand ProductPriceActCmd
            => _productPriceActCmd ?? (_productPriceActCmd = new AsyncCommand(ProductPriceAct));

        private async Task ProductPriceAct(object obj)
        {
            try
            {
                if (obj is Window window)
                {
                    _productPrice.ProductId = SelectedProduct.Id;
                    _productPrice.Cost = Cost;
                    _productPrice.Date = Date.GetUnixTime();

                    if (IsEdit)
                    {
                        await _productPriceService.EditProductPrice(_productPrice);
                    }
                    else
                    {
                        await _productPriceService.AddProductPrice(_productPrice);
                    }

                    window.DialogResult = false;
                }
            }
            catch (System.Exception ex)
            {
                await _dialogService.ShowErrorDialog(ex.Message, this);
            }
        }
        #endregion

        #region CloseCmd

        private ICommand _closeCmd;

        public ICommand CloseCmd
            => _closeCmd ?? (_closeCmd = new RelayCommand(Close));

        private void Close(object obj)
        {

            if (obj is Window window)
            {
                window.DialogResult = false;
            }
        }
        #endregion
        #endregion
        #endregion
    }
}