namespace ApiApp.Models
{
    public class OperationProduct
    {
        public int ProductId { get; set; }

        public Product Product { get; set; }

        public int Count { get; set; }
    }
}