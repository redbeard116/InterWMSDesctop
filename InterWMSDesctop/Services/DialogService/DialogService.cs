using ApiApp.Models;
using ApiApp.Services.ContractService;
using ApiApp.Services.CounterpartyService;
using ApiApp.Services.DictionaryService;
using ApiApp.Services.ProductPriceService;
using ApiApp.Services.ProductService;
using ApiApp.Services.StorageAreaService;
using ApiApp.Services.UserService;
using InterWMSDesctop.ViewModels;
using InterWMSDesctop.ViewModels.Acts;
using InterWMSDesctop.Views;
using InterWMSDesctop.Views.Act;
using MahApps.Metro.Controls.Dialogs;
using System.Threading.Tasks;

namespace InterWMSDesctop.Services.DialogService
{
    public class DialogService : IDialogService
    {
        #region Fields
        private readonly IDialogCoordinator _dialogCoordinator;
        private readonly UserActVM _userActVM;
        private readonly ContractActVM _contractVM;
        private readonly CounterpartyActVM _counterpartyActVM;
        private readonly ProductActVM _productActVM;
        private readonly ProductPriceActVM _productPriceActVM;
        private readonly object _context;
        #endregion

        #region Constructor
        public DialogService(IDialogCoordinator dialogCoordinator,
                             IUserService userService,
                             IContractService contractService,
                             ICounterpartyService counterpartyService,
                             IProductService productService,
                             IDictionaryService dictionaryService,
                             IStorageAreaService storageAreaService,
                             IProductPriceService productPrice,
                             object context)
        {
            _dialogCoordinator = dialogCoordinator;
            _context = context;
            _userActVM = new UserActVM(userService, this);
            _contractVM = new ContractActVM(contractService, counterpartyService, productService, productPrice,this);
            _counterpartyActVM = new CounterpartyActVM(counterpartyService, this);
            _productActVM = new ProductActVM(productService, dictionaryService, storageAreaService, this);
            _productPriceActVM = new ProductPriceActVM(productPrice, productService, this);
        }
        #endregion

        #region IDialogService
        public string InputDialog(string title, string message, string defaultText = null)
        {
            return _dialogCoordinator.ShowModalInputExternal(_context, title, message, new MetroDialogSettings
            {
                DefaultText = defaultText
            });
        }


        public bool? OpenEditCounterparty(Counterparty counterparty, bool isEdit = false)
        {
            _counterpartyActVM.Load(counterparty, isEdit);

            var view = new CounterpartyActV
            {
                DataContext = _counterpartyActVM
            };

            return view.ShowDialog();
        }

        public async Task<bool?> OpenEditContract(Contract contract, bool isEdit = false)
        {
            await _contractVM.Load(contract, isEdit);

            var view = new ContractActV
            {
                DataContext = _contractVM
            };

            return view.ShowDialog();
        }

        public async Task<bool?> OpenEditUser(int? userId, bool isEdit = false)
        {
            await _userActVM.Load(userId, isEdit);

            var userActV = new UserActV
            {
                DataContext = _userActVM
            };

            return userActV.ShowDialog();
        }

        public async Task ShowErrorDialog(string message, object context = null)
        {
            await _dialogCoordinator.ShowMessageAsync(context ?? _context, "Ошибка", message);
        }

        public async Task<bool?> OpenEditProduct(Product product, bool isEdit = false)
        {
            await _productActVM.Load(product, isEdit);

            var view = new ProductActV
            {
                DataContext = _productActVM
            };

            return view.ShowDialog();
        }

        public async Task<bool?> OpenEditProductPrice(ProductPrice productPrice, bool isEdit = false)
        {
            await _productPriceActVM.Load(productPrice, isEdit);

            var view = new ProductPriceActV
            {
                DataContext = _productActVM
            };

            return view.ShowDialog();
        }
        #endregion
    }
}
