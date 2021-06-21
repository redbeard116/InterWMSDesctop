using ApiApp.Models;
using ApiApp.Services.CounterpartyService;
using InterWMSDesctop.Services.DialogService;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace InterWMSDesctop.ViewModels.Acts
{
    class CounterpartyActVM : ViewModelBase
    {
        #region Fields
        private readonly ICounterpartyService _counterpartyService;
        private readonly IDialogService _dialogService;

        private Counterparty _counterparty;
        private bool _isEdit;

        private int _account;
        private int _iNN;
        private string _firstName;
        private string _secondName;
        private string _number;
        private string _middleName;
        #endregion

        #region Constructor
        public CounterpartyActVM(ICounterpartyService counterpartyService,
                                 IDialogService dialogService)
        {
            _counterpartyService = counterpartyService;
            _dialogService = dialogService;
        }
        #endregion

        #region Properties
        public bool IsEdit => _isEdit;

        public int Account
        {
            get => _account;
            set => OnPropertyChanged(ref _account, value, () => Account);
        }
        public int INN
        {
            get => _iNN;
            set => OnPropertyChanged(ref _iNN, value, () => INN);
        }
        public string FirstName
        {
            get => _firstName;
            set => OnPropertyChanged(ref _firstName, value, () => FirstName);
        }
        public string SecondName
        {
            get => _secondName;
            set => OnPropertyChanged(ref _secondName, value, () => SecondName);
        }
        public string Number
        {
            get => _number;
            set => OnPropertyChanged(ref _number, value, () => Number);
        }
        public string MiddleName
        {
            get => _middleName;
            set => OnPropertyChanged(ref _middleName, value, () => MiddleName);
        }

        public string ButtonContent => IsEdit ? "Применить" : "Создать";
        #endregion

        #region Public methods
        public void Load(Counterparty counterparty, bool isEdit)
        {
            _isEdit = isEdit;
            if (_isEdit)
            {
                _counterparty = counterparty;
            }
            else
            {
                _counterparty = new Counterparty
                {
                    User = new User()
                };

                _counterparty.User.Role = UserRole.Counterparty;
            }

            Account = _counterparty.Account;
            INN = _counterparty.INN;
            FirstName = _counterparty?.User?.FirstName;
            SecondName = _counterparty?.User?.SecondName;
            Number = _counterparty?.User?.Number;
            MiddleName = _counterparty?.User?.MiddleName;

            OnPropertyChanged(() => IsEdit);
        }
        #endregion

        #region Commands
        #region CounterpartyActCmd

        private ICommand _userActCmd;

        public ICommand CounterpartyActCmd
            => _userActCmd ?? (_userActCmd = new AsyncCommand(UserAct));

        private async Task UserAct(object obj)
        {
            try
            {
                if (obj is Window window)
                {
                    _counterparty.Account = Account;
                    _counterparty.INN = INN;
                    _counterparty.User.FirstName = FirstName;
                    _counterparty.User.SecondName = SecondName;
                    _counterparty.User.Number = Number;
                    _counterparty.User.MiddleName = MiddleName;

                    if (IsEdit)
                    {
                        await _counterpartyService.EditCounterparty(_counterparty);
                    }
                    else
                    {
                        await _counterpartyService.AddCounterparty(_counterparty);
                    }

                    window.DialogResult = true;
                }
            }
            catch (System.Exception ex)
            {
                await _dialogService.ShowErrorDialog(ex.Message, this);
            }
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
