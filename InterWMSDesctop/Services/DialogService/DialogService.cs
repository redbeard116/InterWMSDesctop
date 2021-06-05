using ApiApp.Services.UserService;
using InterWMSDesctop.ViewModels;
using InterWMSDesctop.Views;
using MahApps.Metro.Controls.Dialogs;
using System.Threading.Tasks;

namespace InterWMSDesctop.Services.DialogService
{
    public class DialogService : IDialogService
    {
        #region Fields
        private readonly IDialogCoordinator _dialogCoordinator;
        private readonly UserActVM _userActVM;
        private readonly object _context;
        #endregion

        #region Constructor
        public DialogService(IDialogCoordinator dialogCoordinator,
                             IUserService userService,
                             object context)
        {
            _dialogCoordinator = dialogCoordinator;
            _context = context;
            _userActVM = new UserActVM(userService, this);
        }
        #endregion

        #region IDialogService
        public async Task<bool?> OpenEditUser(int? userId, bool isEdit = false)
        {
            var customDialog = new CustomDialog();

            await _userActVM.Load(userId, isEdit);

            var userActV = new UserActV
            {
                DataContext = _userActVM
            };

            return userActV.ShowDialog();
        }

        public async Task ShowErrorDialog(string message)
        {
            await _dialogCoordinator.ShowMessageAsync(_context, "Ошибка", message);
        }
        #endregion
    }
}
