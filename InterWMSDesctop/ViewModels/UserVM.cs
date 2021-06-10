using ApiApp.Models;
using ApiApp.Services.UserService;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace InterWMSDesctop.ViewModels
{
    public class UserVM : ViewModelBase
    {
        #region Fields
        private IUserService _userService;
        private User _currUser;
        private UserRole _selectedRole;
        #endregion

        #region Constructor
        public UserVM(IUserService userService)
        {
            _userService = userService;
        }
        #endregion

        #region PublicMethods
        public override async Task Load()
        {
            _currUser = await _userService.GetCurrentUser();
            SelectedRole = _currUser.Role;
            OnPropertyChanged(nameof(User));
        }
        #endregion

        #region Properties
        public UserRole SelectedRole
        {
            get => _selectedRole;
            set => OnPropertyChanged(ref _selectedRole, value, () => SelectedRole);
        }
        public User User => _currUser;
        #endregion

        #region Commands
        #region EditUserCmd

        private ICommand _editUserCmd;

        public ICommand EditUserCmd
            => _editUserCmd ?? (_editUserCmd = new AsyncCommand(EditUser));

        private async Task EditUser(object obj)
        {
            if (_currUser == null)
                return;

            try
            {
                if (obj is PasswordBox passwordBox && !string.IsNullOrWhiteSpace(passwordBox.Password))
                {
                    _currUser.Password = passwordBox.Password;
                    _currUser = await _userService.EditUser(_currUser);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion
        #endregion
    }
}
