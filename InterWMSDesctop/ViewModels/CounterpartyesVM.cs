using ApiApp.Models;
using ApiApp.Services.CounterpartyService;
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

        private IEnumerable<Counterparty> _counterparties;
        private Counterparty _selectesCounterparty;
        private Counterparty _newCounterparty;
        private User _user;
        #endregion

        #region Constructor
        public CounterpartyesVM(ICounterpartyService counterpartyService)
        {
            _counterpartyService = counterpartyService;
        }
        #endregion

        #region Properties
        public IEnumerable<Counterparty> Counterparties => _counterparties;

        public Counterparty SelectedCounterparty
        {
            get => _selectesCounterparty;
            set => OnPropertyChanged(ref _selectesCounterparty, value, () => SelectedCounterparty);
        }
        public Counterparty NewCounterparty
        {
            get => _newCounterparty;
            set => OnPropertyChanged(ref _newCounterparty, value, () => NewCounterparty);
        }

        public User User
        {
            get => _user;
            set => OnPropertyChanged(ref _user, value, () => User);
        }

        public Visibility NewCounterpartyVisibility { get; private set; } = Visibility.Collapsed;
        #endregion

        #region Public methods
        public override async Task Load()
        {
            _counterparties = await _counterpartyService.GetCounterpartyes();
            OnPropertyChanged(() => Counterparties);
        }
        #endregion

        #region Commands
        #region CreateCmd
        private ICommand _createCmd;

        public ICommand CreateCmd
            => _createCmd ?? (_createCmd = new RelayCommand(Create));

        private void Create(object obj)
        {
            NewCounterparty = new Counterparty();
            User = new User();
            NewCounterpartyVisibility = Visibility.Visible;
            OnPropertyChanged(() => NewCounterpartyVisibility);
        }
        #endregion

        #region AddCmd
        private ICommand _addCmd;

        public ICommand AddCmd
            => _addCmd ?? (_addCmd = new AsyncCommand(Add, CanAdd));

        private bool CanAdd(object obj)
        {
            return NewCounterparty != null;
        }

        private async Task Add(object obj)
        {
            try
            {
                User.Role = UserRole.Counterparty;
                NewCounterparty.User = User;
                var result = await _counterpartyService.AddCounterparty(NewCounterparty);

                if (result != null)
                {
                    await Load();
                    NewCounterparty = null;
                    User = null;
                    NewCounterpartyVisibility = Visibility.Collapsed;
                    OnPropertyChanged(() => NewCounterpartyVisibility);
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
            if (SelectedCounterparty != null)
            {
                var result = await _counterpartyService.EditCounterparty(SelectedCounterparty);
                if (result != null)
                {
                    await Load();
                    SelectedCounterparty = null;
                }
            }
        }
        #endregion
        #endregion
    }
}
