using ApiApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiApp.Services.CounterpartyService
{
    public interface ICounterpartyService
    {
        Task<IEnumerable<Counterparty>> GetCounterpartyes();

        Task<Counterparty> GetCounterparty(int id);

        Task<Counterparty> AddCounterparty(Counterparty counterparty);
        Task<bool> DeleteCounterparty(int id);
        Task<Counterparty> EditCounterparty(Counterparty counterparty);
    }
}
