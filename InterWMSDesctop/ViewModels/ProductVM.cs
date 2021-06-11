using ApiApp.Models;
using ApiApp.Services.ProductService;
using InterWMSDesctop.Services.DialogService;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InterWMSDesctop.ViewModels
{
    class ProductVM : ViewModelBase
    {
        #region Fields
        private readonly IProductService _productServices;
        private readonly IDialogService _dialogService;

        private IEnumerable<Product> _products;
        private Product _selectedProduct;
        private Product _newProduct;
        #endregion

        #region Constructor
        public ProductVM(IProductService productService,
                         IDialogService dialogService)
        {
            _productServices = productService;
            _dialogService = dialogService;
        }
        #endregion

        #region Properties
        public IEnumerable<Product> Products => _products;
        #endregion

        #region Public methods
        public override async Task Load()
        {
            _products = await _productServices.GetProducts();
            OnPropertyChanged(() => Products);
        }
        #endregion

        #region Commands
        #region AddCmd
        private ICommand _addCmd;

        public ICommand AddCmd
            => _addCmd ?? (_addCmd = new AsyncCommand(Add, CanAdd));

        private bool CanAdd(object obj)
        {
            return true;
        }

        private async Task Add(object obj)
        {
            var result = await _dialogService.OpenEditProduct(null);

            if (result == true)
            {
                await Load();
            }
        }
        #endregion

        #region DeleteCmd
        private ICommand _deleteCmd;

        public ICommand DeleteCmd
            => _deleteCmd ?? (_deleteCmd = new AsyncCommand(Delete));

        private async Task Delete(object obj)
        {
            if (obj is Product product)
            {
                var result = await _productServices.DeleteProduct(product.Id);
                if (result)
                {
                    await Load();
                }
            }
        }
        #endregion

        #region EditCmd
        private ICommand _editCmd;

        public ICommand EditCmd
            => _editCmd ?? (_editCmd = new AsyncCommand(Edit));

        private async Task Edit(object obj)
        {
            if (obj is Product product)
            {
                var result = await _dialogService.OpenEditProduct(product, true);

                if (result == true)
                {
                    await Load();
                }
            }
        }
        #endregion
        #endregion
    }
}
