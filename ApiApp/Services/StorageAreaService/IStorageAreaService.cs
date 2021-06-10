using ApiApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiApp.Services.StorageAreaService
{
    public interface IStorageAreaService
    {
        Task<IEnumerable<StorageArea>> GetStorageAreas();

        Task<StorageArea> GetStorageArea(int id);

        Task<StorageArea> AddStorageArea(StorageArea storageArea);
        Task<bool> DeleteStorageArea(int id);
        Task<StorageArea> EditStorageArea(StorageArea storageArea);
    }
}
