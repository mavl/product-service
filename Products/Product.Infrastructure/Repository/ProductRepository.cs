using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Product.Domain.Exceptions;
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
            var result = _mapper.Map<Product.Domain.Product>(
                await _productDatabaseContext.Product.AsQueryable().SingleOrDefaultAsync(product => product.Id == productId));
            
            if(result == null)throw new NotFoundException();
            
            return result;
        }

        public async Task Update(long id, JsonPatchDocument productPatch)
        {
            var product = await _productDatabaseContext.Product.SingleOrDefaultAsync(p => p.Id == id);

            if (product == null) throw new NotFoundException();

            productPatch.ApplyTo(product);

            await _productDatabaseContext.SaveChangesAsync();
        }
    }
}