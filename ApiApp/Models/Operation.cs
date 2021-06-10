namespace ApiApp.Models
{
    public class Operation: JsonBase
    {
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
