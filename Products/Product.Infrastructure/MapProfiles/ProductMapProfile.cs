using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;

namespace Products.Infrastructure.MapProfiles
{
    public class ProductMapProfile:Profile
    {
        public ProductMapProfile()
        {
            CreateMap<Entities.Product, Product.Domain.Product>().ReverseMap();
            
            CreateMap<JsonPatchDocument<Product.Domain.Product>, JsonPatchDocument<Entities.Product>>();
        }
    }
}