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
using System.Windows.Input;
using System.Windows.Threading;

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
        private ProductsTypeVM _productsTypeVM;
        private StorageAreaVM _storageAreaVM;
        private CounterpartyesVM _counterpartyesVM;
        private ProductPriceVM _productPriceVM;
        private ProductVM _productVM;
        private ContractVM _contractVM;
        private ReportsVM _reportsVM;
        private IDialogService _dialogService;

        private MenuItem _selectMenuItem;
        private MenuItem _selectOptionsMenuItem;
        private DispatcherTimer _menuTimer;
        private DispatcherTimer _menuOptionTimer;
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
        public override async Task Load()
        {
            _dialogService = AppServices.Instance.GetService< IDialogService>();
            _userVM = AppServices.Instance.GetService<UserVM>();
            _productsTypeVM = AppServices.Instance.GetService<ProductsTypeVM>();
            _storageAreaVM = AppServices.Instance.GetService<StorageAreaVM>();
            _counterpartyesVM = AppServices.Instance.GetService<CounterpartyesVM>();
            _productPriceVM = AppServices.Instance.GetService<ProductPriceVM>();
            _productVM = AppServices.Instance.GetService<ProductVM>();
            _contractVM = AppServices.Instance.GetService<ContractVM>();
            _reportsVM = AppServices.Instance.GetService<ReportsVM>();

            _usersVM = AppServices.Instance.GetService<UsersVM>(); 

            Menu.Add(new MenuItem()
            {
                Icon = new PackIconFeatherIcons() { Kind = PackIconFeatherIconsKind.Users },
                Label = "Пользователи",
                NavigationType = typeof(UsersV),
                DataContext = _usersVM,
                NavigationDestination = new Uri("Views/UsersV.xaml", UriKind.RelativeOrAbsolute)
            });

            Menu.Add(new MenuItem()
            {
                Icon = new PackIconFeatherIcons() { Kind = PackIconFeatherIconsKind.Type },
                Label = "Типы продуктов",
                NavigationType = typeof(ProductsTypeV),
                DataContext = _productsTypeVM,
                NavigationDestination = new Uri("Views/ProductsTypeV.xaml", UriKind.RelativeOrAbsolute)
            });

            Menu.Add(new MenuItem()
            {
                Icon = new PackIconFeatherIcons() { Kind = PackIconFeatherIconsKind.Book },
                Label = "Места хранения",
                NavigationType = typeof(StorageAreaV),
                DataContext = _storageAreaVM,
                NavigationDestination = new Uri("Views/StorageAreaV.xaml", UriKind.RelativeOrAbsolute)
            });

            Menu.Add(new MenuItem()
            {
                Icon = new PackIconFeatherIcons() { Kind = PackIconFeatherIconsKind.Command },
                Label = "Контрагенты",
                NavigationType = typeof(CounterpartyesV),
                DataContext = _counterpartyesVM,
                NavigationDestination = new Uri("Views/CounterpartyesV.xaml", UriKind.RelativeOrAbsolute)
            });

            Menu.Add(new MenuItem()
            {
                Icon = new PackIconFeatherIcons() { Kind = PackIconFeatherIconsKind.Airplay },
                Label = "Цены",
                NavigationType = typeof(ProductPriceV),
                DataContext = _productPriceVM,
                NavigationDestination = new Uri("Views/ProductPriceV.xaml", UriKind.RelativeOrAbsolute)
            });

            Menu.Add(new MenuItem()
            {
                Icon = new PackIconFeatherIcons() { Kind = PackIconFeatherIconsKind.Package },
                Label = "Товары",
                NavigationType = typeof(ProductV),
                DataContext = _productVM,
                NavigationDestination = new Uri("Views/ProductV.xaml", UriKind.RelativeOrAbsolute)
            });

            Menu.Add(new MenuItem()
            {
                Icon = new PackIconFeatherIcons() { Kind = PackIconFeatherIconsKind.Coffee },
                Label = "Договора",
                NavigationType = typeof(ContractV),
                DataContext = _contractVM,
                NavigationDestination = new Uri("Views/ContractV.xaml", UriKind.RelativeOrAbsolute)
            });

            OptionsMenu.Add(new MenuItem()
            {
                Icon = new PackIconFeatherIcons() { Kind = PackIconFeatherIconsKind.Repeat },
                Label = "Отчеты",
                NavigationType = typeof(ReportsV),
                DataContext = _reportsVM,
                NavigationDestination = new Uri("Views/ReportsV.xaml", UriKind.RelativeOrAbsolute)
            });

            OptionsMenu.Add(new MenuItem()
            {
                Icon = new PackIconFeatherIcons() { Kind = PackIconFeatherIconsKind.User },
                Label = _userProvider.Name,
                NavigationType = typeof(UserV),
                DataContext = _userVM,
                NavigationDestination = new Uri("Views/UserV.xaml", UriKind.RelativeOrAbsolute)
            });


            _menuTimer = new DispatcherTimer();
            _menuTimer.Tick += async (e, sender) => await LoadItems();

            _menuOptionTimer = new DispatcherTimer();
            _menuOptionTimer.Tick += async (e, sender) => await LoadOptionItems();
        }

        public async Task LoadItems()
        {
            _menuTimer.Stop();
            if (SelectedMenuItem == null)
            {
                return;
            }

            if (SelectedMenuItem.DataContext is ViewModelBase modelBase)
            {
                await modelBase.Load();
            }
        }

        public async Task LoadOptionItems()
        {
            _menuOptionTimer.Stop();

            if (SelecteOptionsdMenuItem == null)
            {
                return;
            }

            if (SelecteOptionsdMenuItem.DataContext is ViewModelBase modelBase && OptionsMenu.Contains(SelecteOptionsdMenuItem))
            {
                await modelBase.Load();
            }
        }
        #endregion

        #region Properties
        public ObservableCollection<MenuItem> Menu => AppMenu;

        public MenuItem SelectedMenuItem
        {
            get => _selectMenuItem;
            set
            {
                OnPropertyChanged(ref _selectMenuItem, value, () => SelectedMenuItem);
                _menuTimer.Start();
            }
        }

        public MenuItem SelecteOptionsdMenuItem
        {
            get => _selectOptionsMenuItem;
            set
            {
                OnPropertyChanged(ref _selectOptionsMenuItem, value, () => SelecteOptionsdMenuItem);
                _menuOptionTimer.Start();
            }
        }

        public ObservableCollection<MenuItem> OptionsMenu => AppOptionsMenu;
        #endregion


        #region Commands

        #endregion
    }
}
