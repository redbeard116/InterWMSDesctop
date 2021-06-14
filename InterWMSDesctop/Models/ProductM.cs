using ApiApp.Models;
using InterWMSDesctop.ViewModels;

namespace InterWMSDesctop.Models
{
    class ProductM : ViewModelBase
    {
        #region Fields
        private Product _product;
        private int _count;
        #endregion

        #region Constructor
        public ProductM(Product product)
        {
            _product = product;
        }
        #endregion

        #region Properties
        public int Id => _product.Id;
        public string Name => _product.Name;
        public Product Product
        {
            get => _product;
            set => OnPropertyChanged(ref _product, value, () => Product);
        }

        public int Count
        {
            get => _count;
            set => OnPropertyChanged(ref _count, value, () => Count);
        }
        #endregion
    }
}
