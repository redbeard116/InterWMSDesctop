using ApiApp.Models;
using ApiApp.Services.ProductService;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InterWMSDesctop.ViewModels
{
    class ProductVM : ViewModelBase
    {
        #region Fields
        private readonly IProductService _productServices;

        private IEnumerable<Product> _products;
        private Product _selectedProduct;
        private Product _newProduct;
        #endregion

        #region Constructor
        public ProductVM(IProductService productService)
        {
            _productServices = productService;
        }
        #endregion

        #region Properties
        public IEnumerable<Product> Products => _products;

        public Product SelectedProduct
        {
            get => _selectedProduct;
            set => OnPropertyChanged(ref _selectedProduct, value, () => SelectedProduct);
        }

        public Product NewProduct
        {
            get => _newProduct;
            set => OnPropertyChanged(ref _newProduct, value, () => NewProduct);
        }
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
            var result = await _productServices.AddProduct(NewProduct);

            if (result != null)
            {
                await Load();
                NewProduct = null;
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
            if (SelectedProduct != null)
            {
                var result = await _productServices.EditProduct(SelectedProduct);
                if (result != null)
                {
                    await Load();
                    SelectedProduct = null;
                }
            }
        }
        #endregion
        #endregion

        #region Private methods

        #endregion
    }
}
