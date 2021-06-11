using MahApps.Metro.Controls;
using System.Windows;

namespace InterWMSDesctop.Views
{
    /// <summary>
    /// Логика взаимодействия для UserActV.xaml
    /// </summary>
    public partial class UserActV : MetroWindow
    {
        public UserActV()
        {
            InitializeComponent();
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void Apply(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
