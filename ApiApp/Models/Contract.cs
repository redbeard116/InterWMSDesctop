using System.Collections.Generic;
using System.ComponentModel;

namespace ApiApp.Models
{
    public class Contract : JsonBase
    {
        public Counterparty Counterparty { get; set; }

        public long Date { get; set; }

        public double Sum { get; set; }
        public List<OperationProduct> Products { get; set; }

        public OperationType Type { get; set; }

        public int Count { get; set; }
    }

    public enum OperationType
    {
        Reception,
        Shipping
    }
}
