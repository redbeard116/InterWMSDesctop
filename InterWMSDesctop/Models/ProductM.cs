using ApiApp.Models;
using InterWMSDesctop.ViewModels;
using System.ComponentModel;

namespace InterWMSDesctop.Models
{
    class ProductM : ViewModelBase, IDataErrorInfo
    {
        #region Fields
        private Product _product;
        private ProductInfo _productInfo;
        private int _count;
        private OperationType _operationType;
        private double _cost;
        #endregion

        #region Constructor
        public ProductM(Product product, ProductInfo productInfo)
        {
            _product = product;
            _productInfo = productInfo ?? new ProductInfo();
            _operationType = OperationType.Reception;
            OnPropertyChanged(() => OperationType);
        }

        #endregion

        #region Public methods
        public void SetOperationType(OperationType operationType)
        {
            _operationType = operationType;
            OnPropertyChanged(() => OperationType);
        }

        public void Clear()
        {
            Cost = 0;
            Count = 0;
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
            set
            {
                OnPropertyChanged(ref _count, value, () => Count);
                OnPropertyChanged(() => Sum);
            }
        }
        public double Cost
        {
            get => _cost;
            set
            {
                OnPropertyChanged(ref _cost, value, () => Cost);
                OnPropertyChanged(() => Sum);
            }
        }
        public OperationType OperationType => _operationType;
        public int MaxCount => _productInfo.Count;
        public double Sum => Count * (OperationType == OperationType.Shipping ? _productInfo.Cost : Cost);
        #endregion

        #region IDataErrorInfo
        public string this[string columnName]
        {
            get
            {
                if (columnName.Equals("Count"))
                {
                    if (_operationType == OperationType.Reception)
                    {
                        return null;
                    }

                    if (Count > MaxCount)
                    {
                        return $"Максимальное количество {MaxCount}";
                    }
                }

                return null;
            }
        }

        public string Error => this["Count"];
        #endregion
    }
}
