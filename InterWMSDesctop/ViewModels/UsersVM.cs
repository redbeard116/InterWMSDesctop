using ApiApp.Models;
using ApiApp.Services.UserService;
using InterWMSDesctop.Services.DialogService;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InterWMSDesctop.ViewModels
{
    public class UsersVM : ViewModelBase
    {
        #region Fields
        private IUserService _userService;
        private IDialogService _dialogService;
        private User _selectedUser;
        private IEnumerable<User> _users;
        #endregion

        #region Constructor
        public UsersVM(IUserService userService,
                       IDialogService dialogService)
        {
            _userService = userService;
            _dialogService = dialogService;
        }
        #endregion

        #region Public methods
        public async Task Load()
        {
            _users = await _userService.GetUsers();
            OnPropertyChanged(nameof(Users));
        }
        #endregion

        #region Properties
        public User SelectedUser
        {
            get => _selectedUser;
            set => OnPropertyChanged(ref _selectedUser, value, () => SelectedUser);
        }
        public IEnumerable<User> Users => _users;
        #endregion

        #region Commands
        #region CreateUserCmd

        private ICommand _createUserCmd;

        public ICommand CreateUserCmd
            => _createUserCmd ?? (_createUserCmd = new AsyncCommand(CreateUser));

        private async Task CreateUser(object obj)
        {
            var result = await _dialogService.OpenEditUser(null, false);

            if (result == true)
            {
                await Load();
            }
        }
        #endregion

        #region EditUserCmd

        private ICommand _editUserCmd;

        public ICommand EditUserCmd
            => _editUserCmd ?? (_editUserCmd = new AsyncCommand(EditUser));

        private async Task EditUser(object obj)
        {
            if (obj is User user)
            {
                var result = await _dialogService.OpenEditUser(user.Id, true);

                if (result == true)
                {
                    await Load();
                }
            }
        }
        #endregion

        #region DeleteUserCmd

        private ICommand _deleteUserCmd;

        public ICommand DeleteUserCmd
            => _deleteUserCmd ?? (_deleteUserCmd = new AsyncCommand(DeleteUser));

        private async Task DeleteUser(object obj)
        {
            if (obj is User user)
            {
                var result = await _userService.DeleteUser(user.Id);
                if (result)
                {
                    await Load();
                }
            }
        }
        #endregion
        #endregion
    }
}
