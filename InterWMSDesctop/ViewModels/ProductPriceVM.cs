using ApiApp.Models;
using ApiApp.Services.ProductPriceService;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InterWMSDesctop.ViewModels
{
    class ProductPriceVM : ViewModelBase
    {
        #region Fields
        private readonly IProductPriceService _productPriceService;

        private IEnumerable<ProductPrice> _productPrices;
        private ProductPrice _selectProductPrice;
        private ProductPrice _newProductPrice;
        #endregion

        #region Constructor
        public ProductPriceVM(IProductPriceService productPriceService)
        {
            _productPriceService = productPriceService;
        }
        #endregion

        #region Properties
        public IEnumerable<ProductPrice> ProductPrices => _productPrices;

        public ProductPrice SelectProductPrice
        {
            get => _selectProductPrice;
            set => OnPropertyChanged(ref _selectProductPrice, value, () => SelectProductPrice);
        }

        public ProductPrice NewProductPrice
        {
            get => _newProductPrice;
            set => OnPropertyChanged(ref _newProductPrice, value, () => NewProductPrice);
        }
        #endregion

        #region Public methods
        public override async Task Load()
        {
            _productPrices = await _productPriceService.GetProductPrices();
            OnPropertyChanged(() => ProductPrices);
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
            var result = await _productPriceService.AddProductPrice(NewProductPrice);

            if (result != null)
            {
                await Load();
                NewProductPrice = null;
            }
        }
        #endregion

        #region DeleteCmd
        private ICommand _deleteCmd;

        public ICommand DeleteCmd
            => _deleteCmd ?? (_deleteCmd = new AsyncCommand(Delete));

        private async Task Delete(object obj)
        {
            if (obj is ProductPrice productPrice)
            {
                var result = await _productPriceService.DeleteProductPrice(productPrice.Id);
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
            if (SelectProductPrice != null)
            {
                var result = await _productPriceService.EditProductPrice(SelectProductPrice);
                if (result != null)
                {
                    await Load();
                    SelectProductPrice = null;
                }
            }
        }
        #endregion
        #endregion

        #region Private methods

        #endregion
    }
}