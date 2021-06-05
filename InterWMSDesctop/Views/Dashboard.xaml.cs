using System;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using MahApps.Metro.Controls;
using MenuItem = InterWMSDesctop.ViewModels.MenuItem;

namespace InterWMSDesctop.Views
{
    /// <summary>
    /// Логика взаимодействия для Dashboard.xaml
    /// </summary>
    public partial class Dashboard : MetroWindow
    {
        private readonly NavigationServiceEx navigationServiceEx;

        public Dashboard()
        {
            this.InitializeComponent();

            this.navigationServiceEx = new NavigationServiceEx();
            this.navigationServiceEx.Navigated += this.NavigationServiceEx_OnNavigated;
            this.HamburgerMenuControl.Content = this.navigationServiceEx.Frame;
            NavigationServiceEx = navigationServiceEx;
            // Navigate to the home page.
            this.Loaded += (sender, args) => this.navigationServiceEx.Navigate(new Uri("Views/StartPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void HamburgerMenuControl_OnItemInvoked(object sender, HamburgerMenuItemInvokedEventArgs e)
        {
            if (e.InvokedItem is MenuItem menuItem && menuItem.IsNavigation)
            {
                this.navigationServiceEx.Navigate(menuItem.NavigationDestination);
            }
        }

        private void NavigationServiceEx_OnNavigated(object sender, NavigationEventArgs e)
        {
            // select the menu item

            this.HamburgerMenuControl.SelectedItem = this.HamburgerMenuControl
                                                         .Items
                                                         .OfType<MenuItem>()
                                                         .FirstOrDefault(x => x.NavigationDestination == e.Uri);

            var menuItem = this.HamburgerMenuControl .OptionsItems.OfType<MenuItem>().FirstOrDefault(x => x.NavigationDestination == e.Uri) ?? this.HamburgerMenuControl.Items.OfType<MenuItem>().FirstOrDefault(x => x.NavigationDestination == e.Uri);

            this.HamburgerMenuControl.SelectedOptionsItem = menuItem;

            if (menuItem?.DataContext != null)
            {
                (e.Content as FrameworkElement).DataContext = menuItem.DataContext;
            }


            this.GoBackButton.Visibility = this.navigationServiceEx.CanGoBack ? Visibility.Visible : Visibility.Collapsed;
        }

        private void GoBack_OnClick(object sender, RoutedEventArgs e)
        {
            this.navigationServiceEx.GoBack();
        }


        public static NavigationServiceEx NavigationServiceEx { get; set; }
    }
}
