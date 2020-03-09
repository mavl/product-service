namespace Product.Domain
{
    /// <summary>
    /// Product DTO object
    /// </summary>
    public class Product
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string ImgUri { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }
    }
}
