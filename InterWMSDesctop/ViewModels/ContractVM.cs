using ApiApp.Models;
using ApiApp.Services.ContractService;
using InterWMSDesctop.Services.DialogService;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InterWMSDesctop.ViewModels
{
    class ContractVM : ViewModelBase
    {
        #region Fields
        private readonly IContractService _contractService;
        private readonly IDialogService _dialogService;

        private IEnumerable<Contract> _contracts;
        private Contract _newOperation;
        #endregion

        #region Constructor
        public ContractVM(IContractService contractService,
                          IDialogService dialogService)
        {
            _contractService = contractService;
            _dialogService = dialogService;
        }
        #endregion

        #region Properties
        public IEnumerable<Contract> Contracts => _contracts;

        public Contract NewContract
        {
            get => _newOperation;
            set => OnPropertyChanged(ref _newOperation, value, () => NewContract);
        }
        #endregion

        #region Public methods
        public override async Task Load()
        {
            _contracts = await _contractService.GetContracts();
            OnPropertyChanged(() => Contracts);
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
            var result = await _dialogService.OpenEditContract(null);

            if (result == true)
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
            if (obj is Contract contract)
            {
                var result = await _contractService.DeleteContract(contract.Id);
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
            if (obj is Contract contract)
            {
                var result = await _dialogService.OpenEditContract(contract, true);
                if (result == true)
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
