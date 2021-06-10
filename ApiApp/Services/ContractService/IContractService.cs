using System.Collections.Generic;
using System.Threading.Tasks;
using ApiApp.Models;

namespace ApiApp.Services.ContractService
{
    public interface IContractService
    {
        Task<IEnumerable<Contract>> GetContracts();

        Task<Contract> GetContract(int id);

        Task<Contract> AddContract(Contract contract);
        Task<bool> DeleteContract(int id);
        Task<Contract> EditContract(Contract contract);
    }
}
