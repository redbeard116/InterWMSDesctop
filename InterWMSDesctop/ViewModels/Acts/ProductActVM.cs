using ApiApp.Models;
using ApiApp.Services.DictionaryService;
using ApiApp.Services.ProductService;
using InterWMSDesctop.Services.DialogService;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace InterWMSDesctop.ViewModels.Acts
{
    class ProductActVM : ViewModelBase
    {
        #region Fields
        private readonly IProductService _productService;
        private readonly IDictionaryService _dictionaryService;
        private readonly IDialogService _dialogService;

        private Product _product;
        private bool _isEdit;
        private IEnumerable<ProductType> _productTypes;
        private string _name;
        private ProductType _productType;
        #endregion

        #region Constructor
        public ProductActVM(IProductService productService, IDictionaryService dictionaryService, IDialogService dialogService)
        {
            _productService = productService;
            _dictionaryService = dictionaryService;
            _dialogService = dialogService;
        }
        #endregion

        #region Properties
        public bool IsEdit => _isEdit;
        public string ButtonContent => IsEdit ? "Применить" : "Создать";
        public IEnumerable<ProductType> ProductTypes => _productTypes;
        public string Name
        {
            get => _name;
            set => OnPropertyChanged(ref _name, value, () => Name);
        }
        public ProductType SelectType
        {
            get => _productType;
            set => OnPropertyChanged(ref _productType, value, () => SelectType);
        }
        #endregion


        #region Public methods
        public async Task Load(Product product, bool isEdit)
        {
            _isEdit = isEdit;
            _productTypes = await _dictionaryService.GetProductTypes();
            if (IsEdit)
            {
                _product = product;
            }
            else
            {
                _product = new Product();
            }

            Name = _product.Name;
            SelectType = _productTypes.FirstOrDefault(w => w.Id == _product.TypeId);
            OnPropertyChanged(() => ProductTypes);
        }
        #endregion

        #region Commands
        #region ProductActCmd

        private ICommand _productActCmd;

        public ICommand ProductActCmd
            => _productActCmd ?? (_productActCmd = new AsyncCommand(ProductAct, CanProductAct));

        private bool CanProductAct(object obj)
        {
            return SelectType != null;
        }

        private async Task ProductAct(object obj)
        {
            try
            {
                if (obj is Window window)
                {
                    _product.Name = Name;
                    _product.TypeId = SelectType.Id;

                    if (IsEdit)
                    {
                        await _productService.EditProduct(_product);
                    }
                    else
                    {
                        await _productService.AddProduct(_product);
                    }

                    window.DialogResult = false;
                }
            }
            catch (System.Exception ex)
            {
                await _dialogService.ShowErrorDialog(ex.Message);
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
    }
}
