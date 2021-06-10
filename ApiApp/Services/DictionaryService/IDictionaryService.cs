using ApiApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiApp.Services.DictionaryService
{
    public interface IDictionaryService
    {
        Task<IEnumerable<AccessType>> GetAccessTypes();
        Task<AccessType> AddAccessType(AccessType accessType);
        Task<bool> DeleteAccessType(int id);

        Task<IEnumerable<ProductType>> GetProductTypes();
        Task<ProductType> AddProductTypes(ProductType productType);
        Task<bool> DeleteProductTypes(int id);

        Task<IEnumerable<UserRole>> GetUserRoles();
        Task<IEnumerable<string>> GetOperationTypes();

        Task<IEnumerable<string>> GetRightsGrids();
    }
}