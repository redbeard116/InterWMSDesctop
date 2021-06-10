using ApiApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiApp.Services.OperationService
{
    public interface IOperationService
    {
        Task<IEnumerable<Operation>> GetOperations();

        Task<Operation> GetOperation(int id);

        Task<Operation> AddOperation(Operation operation);
        Task<bool> DeleteOperation(int id);
        Task<Operation> EditOperation(Operation operation);
    }
}
