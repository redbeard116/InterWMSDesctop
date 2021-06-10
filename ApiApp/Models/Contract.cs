namespace ApiApp.Models
{
    public class Contract : JsonBase
    {
        public int CounterpartyId { get; set; }

        public Counterparty Counterparty { get; set; }

        public int OperationId { get; set; }

        public Operation Operation { get; set; }

        public long Date { get; set; }

        public double Sum { get; set; }
    }
}
