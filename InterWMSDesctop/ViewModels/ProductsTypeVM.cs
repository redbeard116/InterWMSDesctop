using ApiApp.Models;
using ApiApp.Services.DictionaryService;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InterWMSDesctop.ViewModels
{
    class ProductsTypeVM : ViewModelBase
    {
        #region Fields
        private readonly IDictionaryService _dictionaryService;

        private IEnumerable<ProductType> _productTypes;
        private string _name;
        #endregion

        #region Constructor
        public ProductsTypeVM(IDictionaryService dictionaryService)
        {
            _dictionaryService = dictionaryService;
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

        public string Name
        {
            get => _name;
            set => OnPropertyChanged(ref _name, value, () => Name);
        }
        #endregion

        #region Commands
        #region AddCmd
        private ICommand _addCmd;

        public ICommand AddCmd
            => _addCmd ?? (_addCmd = new AsyncCommand(Add, CanAdd));

        private bool CanAdd(object obj)
        {
            return !string.IsNullOrWhiteSpace(Name);
        }

        private async Task Add(object obj)
        {
            var result = await _dictionaryService.AddProductTypes(new ProductType { Name = Name });

            if (result != null)
            {
                await Load();
                Name = null;
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
