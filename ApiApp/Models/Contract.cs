namespace ApiApp.Models
{
    public class Contract : JsonBase
    {
        public int CounterpartyId { get; set; }

        public Counterparty Counterparty { get; set; }

        public long Date { get; set; }

        public double Sum { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }

        public OperationType Type { get; set; }

        public int Count { get; set; }
    }

    public enum OperationType
    {
        Reception,
        Shipping
    }
}
