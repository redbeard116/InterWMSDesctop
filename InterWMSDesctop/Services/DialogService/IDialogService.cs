using ApiApp.Models;
using System.Threading.Tasks;

namespace InterWMSDesctop.Services.DialogService
{
    public interface IDialogService
    {
        Task<bool?> OpenEditUser(int? userId, bool isEdit = false);
        Task ShowErrorDialog(string message);
        Task<bool?> OpenEditContract(Contract contract, bool isEdit = false);
        bool? OpenEditCounterparty(Counterparty counterparty, bool isEdit = false);
        string InputDialog(string title, string message, string defaultText = null);
        Task<bool?> OpenEditProduct(Product product, bool isEdit = false);
    }
}
