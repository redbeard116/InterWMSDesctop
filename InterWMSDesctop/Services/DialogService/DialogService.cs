using ApiApp.Models;
using ApiApp.Services.ContractService;
using ApiApp.Services.CounterpartyService;
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
        private readonly object _context;
        #endregion

        #region Constructor
        public DialogService(IDialogCoordinator dialogCoordinator,
                             IUserService userService,
                             IContractService contractService,
                             ICounterpartyService counterpartyService,
                             object context)
        {
            _dialogCoordinator = dialogCoordinator;
            _context = context;
            _userActVM = new UserActVM(userService, this);
            _contractVM = new ContractActVM(contractService, counterpartyService, this);
            _counterpartyActVM = new CounterpartyActVM(counterpartyService, this);
        }
        #endregion

        #region IDialogService
        public string InputDialog(string title, string message)
        {
            return _dialogCoordinator.ShowModalInputExternal(_context, title, message);
        }


        public bool? OpenEditCounterparty(Counterparty counterparty, bool isEdit = false)
        {
            _counterpartyActVM.Load(counterparty, isEdit);

            var view = new CounterpartyActV
            {
                DataContext = _contractVM
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

        public async Task ShowErrorDialog(string message)
        {
            await _dialogCoordinator.ShowMessageAsync(_context, "Ошибка", message);
        }
        #endregion
    }
}
