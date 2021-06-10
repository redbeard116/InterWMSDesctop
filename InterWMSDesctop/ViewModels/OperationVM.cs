using ApiApp.Models;
using ApiApp.Services.OperationService;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InterWMSDesctop.ViewModels
{
    class OperationVM : ViewModelBase
    {
        #region Fields
        private readonly IOperationService _operationService;

        private IEnumerable<Operation> _operations;
        private Operation _selectOperation;
        private Operation _newOperation;
        #endregion

        #region Constructor
        public OperationVM(IOperationService operationService)
        {
            _operationService = operationService;
        }
        #endregion

        #region Properties
        public IEnumerable<Operation> Operations => _operations;

        public Operation SelectOperation
        {
            get => _selectOperation;
            set => OnPropertyChanged(ref _selectOperation, value, () => SelectOperation);
        }

        public Operation NewOperation
        {
            get => _newOperation;
            set => OnPropertyChanged(ref _newOperation, value, () => NewOperation);
        }
        #endregion

        #region Public methods
        public override async Task Load()
        {
            _operations = await _operationService.GetOperations();
            OnPropertyChanged(() => Operations);
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
            var result = await _operationService.AddOperation(NewOperation);

            if (result != null)
            {
                await Load();
                NewOperation = null;
            }
        }
        #endregion

        #region DeleteCmd
        private ICommand _deleteCmd;

        public ICommand DeleteCmd
            => _deleteCmd ?? (_deleteCmd = new AsyncCommand(Delete));

        private async Task Delete(object obj)
        {
            if (obj is Operation operation)
            {
                var result = await _operationService.DeleteOperation(operation.Id);
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
            if (SelectOperation != null)
            {
                var result = await _operationService.EditOperation(SelectOperation);
                if (result != null)
                {
                    await Load();
                    SelectOperation = null;
                }
            }
        }
        #endregion
        #endregion

        #region Private methods

        #endregion
    }
}
