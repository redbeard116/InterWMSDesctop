using ApiApp.Models;
using ApiApp.Services.DictionaryService;
using InterWMSDesctop.Services.DialogService;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InterWMSDesctop.ViewModels
{
    class ProductsTypeVM : ViewModelBase
    {
        #region Fields
        private readonly IDictionaryService _dictionaryService;
        private readonly IDialogService _dialogService;

        private IEnumerable<ProductType> _productTypes;
        #endregion

        #region Constructor
        public ProductsTypeVM(IDictionaryService dictionaryService,
                              IDialogService dialogService)
        {
            _dictionaryService = dictionaryService;
            _dialogService = dialogService;
        }
        #endregion

        #region PublicMethods
        public override async Task Load()
        {
            _productTypes = await _dictionaryService.GetProductTypes();
            OnPropertyChanged(() => ProductTypes);
        }
        #endregion

        #region Properties
        public IEnumerable<ProductType> ProductTypes => _productTypes;
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
            var name = _dialogService.InputDialog("Создание нового типа","");

            if (string.IsNullOrWhiteSpace(name))
            {
                return;
            }

            var result = await _dictionaryService.AddProductTypes(new ProductType { Name = name });

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
            if (obj is ProductType productType)
            {
                var result = await _dictionaryService.DeleteProductTypes(productType.Id);
                if (result)
                {
                    await Load();
                }
            }
        }
        #endregion
        #endregion
    }
}
