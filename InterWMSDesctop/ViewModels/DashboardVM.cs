using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ApiApp.Providers.UserProvider;
using ApiApp.Services.UserService;
using MahApps.Metro.IconPacks;
using InterWMSDesctop.Views;
using InterWMSDesctop.Services.DialogService;
using MahApps.Metro.Controls.Dialogs;

namespace InterWMSDesctop.ViewModels
{
    public class DashboardVM : ViewModelBase
    {
        #region StaticFields
        private readonly ObservableCollection<MenuItem> AppMenu = new ObservableCollection<MenuItem>();
        private readonly ObservableCollection<MenuItem> AppOptionsMenu = new ObservableCollection<MenuItem>();
        #endregion

        #region Fields
        private IUserProvider _userProvider;
        private IUserService _userService;
        private UserVM _userVM;
        private UsersVM _usersVM;
        private IDialogService _dialogService;
        #endregion

        #region Constructor
        public DashboardVM(IUserProvider userProvider,
                           IUserService userService)
        {
            _userProvider = userProvider;
            _userService = userService;
        }
        #endregion

        #region Public Methods
        public async Task Load()
        {
            _dialogService = new DialogService(DialogCoordinator.Instance, _userService, this);
            _userVM = new UserVM(_userService);
            await _userVM.Load();
            _usersVM = new UsersVM(_userService, _dialogService);
            await _usersVM.Load();

            Menu.Add(new MenuItem()
            {
                Icon = new PackIconFeatherIcons() { Kind = PackIconFeatherIconsKind.Users },
                Label = "Пользователи",
                NavigationType = typeof(UsersV),
                DataContext = UsersVM,
                NavigationDestination = new Uri("Views/UsersV.xaml", UriKind.RelativeOrAbsolute)
            });

            OptionsMenu.Add(new MenuItem()
            {
                Icon = new PackIconFeatherIcons() { Kind = PackIconFeatherIconsKind.User },
                Label = _userProvider.Name,
                NavigationType = typeof(UserV),
                DataContext = UserVM,
                NavigationDestination = new Uri("Views/UserV.xaml", UriKind.RelativeOrAbsolute)
            });
        }
        #endregion

        #region Properties
        public ObservableCollection<MenuItem> Menu => AppMenu;
        public ObservableCollection<MenuItem> OptionsMenu => AppOptionsMenu;
        private UserVM UserVM => _userVM;
        private UsersVM UsersVM => _usersVM;
        #endregion


        #region Commands

        #endregion
    }
}
