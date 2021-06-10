namespace ApiApp.Models
{
    public class Product : JsonBase
    {
        public string Name { get; set; }

        public int TypeId { get; set; }
        public ProductType ProductType { get; set; }
    }
}
