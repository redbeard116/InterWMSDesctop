using ApiApp.Models;
using ApiApp.Providers.UserProvider;
using ApiApp.Services.ContractService;
using InterWMSDesctop.Services;
using InterWMSDesctop.Services.DialogService;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InterWMSDesctop.ViewModels
{
    class ContractVM : ViewModelBase
    {
        #region Fields
        private readonly IContractService _contractService;
        private readonly IUserProvider _userProvider;
        private readonly IDialogService _dialogService;

        private IEnumerable<Contract> _contracts;
        private Contract _selectedContract;
        #endregion

        #region Constructor
        public ContractVM(IContractService contractService,
                          IUserProvider userProvider,
                          IDialogService dialogService)
        {
            _contractService = contractService;
            _userProvider = userProvider;
            _dialogService = dialogService;
        }
        #endregion

        #region Properties
        public IEnumerable<Contract> Contracts => _contracts;

        public Contract SelectedContract
        {
            get => _selectedContract;
            set => OnPropertyChanged(ref _selectedContract, value, () => SelectedContract);
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
            try
            {
                var result = await _dialogService.OpenEditContract(null);

                if (result == true)
                {
                    await Load();
                }
            }
            catch (System.Exception ex)
            {
                await _dialogService.ShowErrorDialog(ex.Message);
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
            => _editCmd ?? (_editCmd = new AsyncCommand(Edit, CanEdit));

        private bool CanEdit(object obj)
        {
            if (obj is Contract contract)
            {
                if (_userProvider.Role == UserRole.Manager && contract.Type == OperationType.Shipping)
                {
                    return false;
                }

                return true;
            }
            return false;
        }

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

        #region SaveExcelCmd
        private ICommand _saveExcelCmd;

        public ICommand SaveExcelCmd
            => _saveExcelCmd ?? (_saveExcelCmd = new AsyncCommand(SaveExcel, CanSaveExcel));

        private bool CanSaveExcel(object obj)
        {
            return SelectedContract != null;
        }

        private async Task SaveExcel(object obj)
        {
            try
            {
                if (!CanSaveExcel(obj))
                {
                    return;
                }

                var excel = new ExcelTemplateService();
                excel.SaveContract(SelectedContract);
            }
            catch (Exception ex)
            {
                await _dialogService.ShowErrorDialog(ex.Message);
            }
        }
        #endregion
        #endregion

        #region Private methods

        #endregion
    }
}
