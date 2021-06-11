using ApiApp.Models;
using ApiApp.Services.CounterpartyService;
using InterWMSDesctop.Services.DialogService;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace InterWMSDesctop.ViewModels
{
    class CounterpartyesVM : ViewModelBase
    {
        #region Fields
        private readonly ICounterpartyService _counterpartyService;
        private readonly IDialogService _dialogService;

        private IEnumerable<Counterparty> _counterparties;
        #endregion

        #region Constructor
        public CounterpartyesVM(ICounterpartyService counterpartyService,
                                IDialogService dialogService)
        {
            _counterpartyService = counterpartyService;
            _dialogService = dialogService;
        }
        #endregion

        #region Properties
        public IEnumerable<Counterparty> Counterparties => _counterparties;
        #endregion

        #region Public methods
        public override async Task Load()
        {
            _counterparties = await _counterpartyService.GetCounterpartyes();
            OnPropertyChanged(() => Counterparties);
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
                var result = _dialogService.OpenEditCounterparty(null);

                if (result != null)
                {
                    await Load();
                }
            }
            catch (System.Exception)
            {
            }
        }
        #endregion

        #region DeleteCmd
        private ICommand _deleteCmd;

        public ICommand DeleteCmd
            => _deleteCmd ?? (_deleteCmd = new AsyncCommand(Delete));

        private async Task Delete(object obj)
        {
            if (obj is Counterparty counterparty)
            {
                var result = await _counterpartyService.DeleteCounterparty(counterparty.Id);
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
            if (obj is Counterparty counterparty)
            {
                var result = _dialogService.OpenEditCounterparty(counterparty, true);
                if (result != null)
                {
                    await Load();
                }
            }
        }
        #endregion
        #endregion
    }
}
