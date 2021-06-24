using InterWMSDesctop.Views;
using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace InterWMSDesctop.ViewModels
{
    public class MainVM : ViewModelBase
    {
        #region Fields
        private readonly AuthStateProvider _authStateProvider;
        private string _login = "admin";
        private string _password = "admin";
        #endregion

        #region Constructor
        public MainVM(AuthStateProvider authStateProvider)
        {
            _authStateProvider = authStateProvider;
        }
        #endregion

        #region Свойства
        public string Login
        {
            get => _login;
            set
            {
                OnPropertyChanged(ref _login, value, () => this._login);
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                OnPropertyChanged(ref _password, value, () => Password);
            }
        }
        #endregion

        #region Команды

        #region ContinueClick

        private ICommand _continueClick;

        public ICommand ContinueClickCmd
            => _continueClick ?? (_continueClick = new AsyncCommand(Continue));

        private async Task Continue(object obj)
        {
            Password = (obj as PasswordBox).Password;

            try
            {
                await _authStateProvider.Login(Login, Password);
                OnClose(EventArgs.Empty);
            }
            catch (Exception e)
            {
                return;
            }
        }

        #endregion
        #endregion
        protected virtual void OnClose(EventArgs e)
        {
            if (Close != null)
            {
                Close(this, e);
            }
        }

        #region События
        public event EventHandler Close;
        public event EventHandler LoginChanged;
        #endregion
    }
}
