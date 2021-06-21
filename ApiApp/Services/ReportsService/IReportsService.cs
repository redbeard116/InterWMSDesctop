using ApiApp.Models;
using System;
using System.Threading.Tasks;

namespace ApiApp.Services.ReportsService
{
    public interface IReportsService
    {
        Task<ProductsReport> GetSalesProductsReport(DateTime startTime, DateTime endTime);
        Task<ProductsReport> GetPurchaseProductsReport(DateTime startTime, DateTime endTime);
    }
}
