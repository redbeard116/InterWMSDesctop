using ApiApp.Models;
using ApiApp.Services.ProductPriceService;
using InterWMSDesctop.Services.DialogService;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InterWMSDesctop.ViewModels
{
    class ProductPriceVM : ViewModelBase
    {
        #region Fields
        private readonly IProductPriceService _productPriceService;
        private readonly IDialogService _dialogService;

        private IEnumerable<ProductPrice> _productPrices;
        #endregion

        #region Constructor
        public ProductPriceVM(IProductPriceService productPriceService, IDialogService dialogService)
        {
            _productPriceService = productPriceService;
            _dialogService = dialogService;
        }
        #endregion

        #region Properties
        public IEnumerable<ProductPrice> ProductPrices => _productPrices;
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
            var result = await _dialogService.OpenEditProductPrice(null);

            if (result != null)
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
            if (obj is ProductPrice productPrice)
            {
                var result = await _dialogService.OpenEditProductPrice(productPrice, true);
                if (result != null)
                {
                    await Load();
                }
            }
        }
        #endregion
        #endregion

        #region Private methods

        #endregion
    }
}