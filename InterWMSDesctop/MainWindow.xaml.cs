using System;
using System.Windows;
using MahApps.Metro.Controls;

namespace InterWMSDesctop
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        internal void Close(object sender, EventArgs e)
        {
            this.DialogResult = true;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            (DataContext as InterWMSDesctop.ViewModels.MainVM).Close += this.Close;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
