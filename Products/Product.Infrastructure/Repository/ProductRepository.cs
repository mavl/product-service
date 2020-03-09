using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Product.Domain.Interfaces;
using Products.Infrastructure.Entities;

namespace Products.Infrastructure.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDatabaseContext _productDatabaseContext;
        private readonly IMapper _mapper;

        public ProductRepository(ProductDatabaseContext productDatabaseContext, IMapper mapper)
        {
            _productDatabaseContext = productDatabaseContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Product.Domain.Product>> GetProducts()
        {
            return _mapper.Map<IEnumerable<Product.Domain.Product>>(
                await _productDatabaseContext.Product.AsQueryable().ToListAsync());
        }

        public async Task<Product.Domain.Product> GetProduct(long productId)
        {
            return _mapper.Map<Product.Domain.Product>(
                await _productDatabaseContext.Product.AsQueryable().SingleAsync(product => product.Id == productId));
        }

        public async Task Update(long id, JsonPatchDocument productPatch)
        {
            var product = await _productDatabaseContext.Product.SingleAsync(p => p.Id == id);
            
            productPatch.ApplyTo(product);

            await _productDatabaseContext.SaveChangesAsync();
        }
    }
}