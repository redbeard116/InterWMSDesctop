namespace ApiApp.Models
{
    public class ProductInfo : JsonBase
    {
        public int ProductId { get; set; }
        public double Cost { get; set; }
        public long Date { get; set; }
        public int Count { get; set; }
    }
}
