using System.Threading.Tasks;

namespace InterWMSDesctop.Services.DialogService
{
    public interface IDialogService
    {
        Task<bool?> OpenEditUser(int? userId, bool isEdit = false);
        Task ShowErrorDialog(string message);
    }
}
