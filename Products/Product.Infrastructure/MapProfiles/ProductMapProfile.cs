using AutoMapper;

namespace Products.Infrastructure.MapProfiles
{
    public class ProductMapProfile:Profile
    {
        public ProductMapProfile()
        {
            CreateMap<Entities.Product, Product.Domain.Product>().ReverseMap();
        }
    }
}