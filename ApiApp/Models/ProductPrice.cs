namespace ApiApp.Models
{
    public class ProductPrice : JsonBase
    {
        public int ProductId { get; set; }

        public Product Product { get; set; }

        public double Cost { get; set; }
    }
}
