using ApiApp.Models;
using ApiApp.Services.UserService;
using InterWMSDesctop.Services.DialogService;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace InterWMSDesctop.ViewModels
{
    public class UserActVM : ViewModelBase
    {
        #region Fields
        private readonly IUserService _userService;
        private readonly IDialogService _dialogService;
        private User _currUser;
        private UserRole _selectedRole;
        private bool _isEdit;
        private string _firstName;
        private string _secondName;
        private string _login;
        #endregion

        #region Constructor
        public UserActVM(IUserService userService,
                         IDialogService dialogService)
        {
            _userService = userService;
            _dialogService = dialogService;
        }
        #endregion

        #region PublicMethods
        public async Task Load(int? userId, bool isEdit)
        {
            _isEdit = isEdit;
            if (userId.HasValue)
            {
                _currUser = await _userService.GetUser(userId.Value);
            }
            else
            {
                _currUser = new User();
            }
            FirstName = _currUser.FirstName;
            SecondName = _currUser.SecondName;
            Login = _currUser.Login;
            SelectedRole = _currUser.Role;
            OnPropertyChanged(nameof(IsEdit));
        }
        #endregion

        #region Properties
        public UserRole SelectedRole
        {
            get => _selectedRole;
            set => OnPropertyChanged(ref _selectedRole, value, () => SelectedRole);
        }
        #region User
        public string FirstName
        {
            get => _firstName;
            set => OnPropertyChanged(ref _firstName, value, () => FirstName);
        }
        public string SecondName
        {
            get => _secondName;
            set => OnPropertyChanged(ref _secondName, value, () => FirstName);
        }
        public string Login
        {
            get => _login;
            set => OnPropertyChanged(ref _login, value, () => FirstName);
        }
        #endregion
        public bool IsEdit => _isEdit;
        public string ButtonContent => IsEdit ? "Применить" : "Создать";
        #endregion

        #region Commands
        #region UserActCmd

        private ICommand _userActCmd;

        public ICommand UserActCmd
            => _userActCmd ?? (_userActCmd = new AsyncCommand(UserAct));

        private async Task UserAct(object obj)
        {
            if (_currUser == null)
                return;

            try
            {
                if (obj is PasswordBox passwordBox)
                {
                    if (!string.IsNullOrWhiteSpace(passwordBox.Password))
                    {
                        _currUser.Password = passwordBox.Password;
                    }

                    if (IsEdit)
                    {
                        _currUser = await _userService.EditUser(_currUser);
                    }
                    else
                    {
                        _currUser.FirstName = FirstName;
                        _currUser.SecondName = SecondName;
                        _currUser.Login = Login;

                        var id = await _userService.AddUser(_currUser);
                        if (id.HasValue)
                        {
                            _currUser.Id = id.Value;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                await _dialogService.ShowErrorDialog(ex.Message);
            }
        }
        #endregion
        #endregion
    }
}
