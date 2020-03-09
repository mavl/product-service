namespace Products.Infrastructure.Entities
{
    public partial class Product
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string ImgUri { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
    }
}
