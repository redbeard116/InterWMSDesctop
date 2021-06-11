using ApiApp.Models;
using ApiApp.Services.ContractService;
using ApiApp.Services.CounterpartyService;
using ApiApp.Services.UserService;
using InterWMSDesctop.Services.DialogService;
using System;
using System.Threading.Tasks;

namespace InterWMSDesctop.ViewModels.Acts
{
    class ContractActVM : ViewModelBase
    {
        public ContractActVM(IContractService contractService, ICounterpartyService counterpartyService, IDialogService dialogService)
        {

        }

        internal Task Load(Contract contract, bool isEdit)
        {
            throw new NotImplementedException();
        }
    }
}
