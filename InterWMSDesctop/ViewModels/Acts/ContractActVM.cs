using ApiApp.Models;
using ApiApp.Services.ContractService;
using ApiApp.Services.CounterpartyService;
using ApiApp.Services.ProductService;
using InterWMSDesctop.Services.DialogService;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ApiApp.Extensions;
using System.Windows.Input;
using System.Windows;
using System.Linq;
using InterWMSDesctop.Models;
using ApiApp.Services.ProductPriceService;
using System.Collections.Generic;

namespace InterWMSDesctop.ViewModels.Acts
{
    class ContractActVM : ViewModelBase
    {
        #region Fields
        private readonly IContractService _contractService;
        private readonly ICounterpartyService _counterpartyService;
        private readonly IProductService _productService;
        private readonly IDialogService _dialogService;
        private readonly IProductPriceService _productPriceService;

        private bool _isEdit;
        private Contract _contract;
        private Counterparty _selectedCounterparty;
        private double _sum;
        private OperationType _selectedType;
        private ProductM _selectedProduct;
        private IEnumerable<ProductPrice> _productPrices;
        #endregion

        #region Constructor
        public ContractActVM(IContractService contractService, ICounterpartyService counterpartyService, IProductService productService, IProductPriceService productPriceService, IDialogService dialogService)
        {
            _contractService = contractService;
            _counterpartyService = counterpartyService;
            _productService = productService;
            _productPriceService = productPriceService;
            _dialogService = dialogService;
        }
        #endregion

        #region Properties
        public bool IsEdit => _isEdit;
        public Counterparty SelectedCounterparty
        {
            get => _selectedCounterparty;
            set => OnPropertyChanged(ref _selectedCounterparty, value, () => SelectedCounterparty);
        }
        public ObservableCollection<Counterparty> Counterparties { get; set; }
        public double Sum
        {
            get => _sum;
            set => OnPropertyChanged(ref _sum, value, () => Sum);
        }
        public ObservableCollection<OperationProduct> AddedProducts { get; set; }
        public ProductM SelectedProduct
        {
            get => _selectedProduct;
            set => OnPropertyChanged(ref _selectedProduct, value, () => SelectedProduct);
        }
        public ObservableCollection<ProductM> Products { get; set; }
        public OperationType SelectedType
        {
            get => _selectedType;
            set => OnPropertyChanged(ref _selectedType, value, () => SelectedType);
        }
        public string ButtonContent => IsEdit ? "Применить" : "Создать";
        #endregion

        #region Public methods
        public async Task Load(Contract contract, bool isEdit)
        {
            _isEdit = isEdit;
            var counterparties = await _counterpartyService.GetCounterpartyes();
            var products = await _productService.GetProducts();
            _productPrices = await _productPriceService.GetLastPrices();
            Counterparties = new ObservableCollection<Counterparty>(counterparties);
            Products = new ObservableCollection<ProductM>(products.Select(w => new ProductM(w)));
            AddedProducts = new ObservableCollection<OperationProduct>();
            if (IsEdit)
            {
                _contract = contract;
                AddedProducts = new ObservableCollection<OperationProduct>(_contract.Products);
                SelectedType = _contract.Type;
                SelectedCounterparty = _contract.Counterparty;
            }
            else
            {
                _contract = new Contract();
            }

            SelectedCounterparty = _contract.Counterparty;
            Sum = _contract.Sum;

            OnPropertyChanged(() => IsEdit);
        }
        #endregion

        #region Commands
        #region ContractActCmd

        private ICommand _contractActCmd;

        public ICommand ContractActCmd
            => _contractActCmd ?? (_contractActCmd = new AsyncCommand(ContractAct));

        private async Task ContractAct(object obj)
        {
            try
            {
                if (obj is Window window)
                {
                    _contract.Date = DateTime.Now.GetUnixTime();
                    _contract.Counterparty = SelectedCounterparty;
                    _contract.Sum = Sum;
                    _contract.Type = SelectedType;
                    _contract.Products = AddedProducts.ToList();

                    if (IsEdit)
                    {
                        await _contractService.EditContract(_contract);
                    }
                    else
                    {
                        await _contractService.AddContract(_contract);
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

        #region AddProductCmd
        private ICommand _addProductCmd;

        public ICommand AddProductCmd
            => _addProductCmd ?? (_addProductCmd = new RelayCommand(AddProduct, CanAddProduct));

        private bool CanAddProduct(object obj)
        {
            return SelectedProduct != null;
        }

        private void AddProduct(object obj)
        {
            if (!CanAddProduct(null))
            {
                return;
            }

            AddedProducts.Add(new OperationProduct
            {
                Product = SelectedProduct.Product,
                ProductId = SelectedProduct.Id,
                Count = SelectedProduct.Count
            });
            Sum += SelectedProduct.Count * _productPrices.FirstOrDefault(w => w.ProductId == SelectedProduct.Id).Cost;
            SelectedProduct = null;
            OnPropertyChanged(() => AddedProducts);
        }
        #endregion

        #region DeleteProductCmd
        private ICommand _deleteProductCmd;

        public ICommand DeleteProductCmd
            => _deleteProductCmd ?? (_deleteProductCmd = new RelayCommand(DeleteProduct, CanDeleteProduct));

        private bool CanDeleteProduct(object obj)
        {
            return obj is OperationProduct;
        }

        private void DeleteProduct(object obj)
        {
            if (!CanDeleteProduct(obj))
            {
                return;
            }

            var product = obj as OperationProduct;
            AddedProducts.Remove(obj as OperationProduct);
            Sum -= product.Count * _productPrices.FirstOrDefault(w => w.ProductId == product.ProductId).Cost;
            OnPropertyChanged(() => AddedProducts);
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
