using System.Collections.Generic;
using System.Linq;

namespace ApiApp.Models
{
    public class ProductsReport : JsonBase
    {
        public string ReportName { get; set; }

        public IEnumerable<ReportProduct> ReportProducts { get; set; }
    }

    public class ReportProduct
    {
        public string Name { get; set; }
        public IEnumerable<ReportProductInfo> Infos { get; set; }
        public int Count => Infos.Sum(w => w.Count);
    }

    public class ReportProductInfo
    {
        public int Count { get; set; }
        public string Date { get; set; }
        public double Price { get; set; }
    }
}
