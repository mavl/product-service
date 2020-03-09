using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Product.Domain.Interfaces;

namespace Product.API.Services
{
    /// <summary>
    /// <see cref="IProductService"/>
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Domain.Product>> GetProducts()
        {
            return await _productRepository.GetProducts();
        }

        public async Task<Domain.Product> GetProduct(long productId)
        {
            return await _productRepository.GetProduct(productId);
        }

        public async Task Update(long id, JsonPatchDocument productPatch)
        {
            await _productRepository.Update(id, productPatch);
        }
    }
}
