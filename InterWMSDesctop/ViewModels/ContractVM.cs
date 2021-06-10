using ApiApp.Models;
using ApiApp.Services.ContractService;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InterWMSDesctop.ViewModels
{
    class ContractVM : ViewModelBase
    {
        #region Fields
        private readonly IContractService _contractService;

        private IEnumerable<Contract> _contracts;
        private Contract _selectOperation;
        private Contract _newOperation;
        #endregion

        #region Constructor
        public ContractVM(IContractService contractService)
        {
            _contractService = contractService;
        }
        #endregion

        #region Properties
        public IEnumerable<Contract> Contracts => _contracts;

        public Contract SelectContract
        {
            get => _selectOperation;
            set => OnPropertyChanged(ref _selectOperation, value, () => SelectContract);
        }

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
            var result = await _contractService.AddContract(NewContract);

            if (result != null)
            {
                await Load();
                NewContract = null;
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
            if (SelectContract != null)
            {
                var result = await _contractService.EditContract(SelectContract);
                if (result != null)
                {
                    await Load();
                    SelectContract = null;
                }
            }
        }
        #endregion
        #endregion

        #region Private methods

        #endregion
    }
}
