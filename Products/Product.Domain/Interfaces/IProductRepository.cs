using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;

namespace Product.Domain.Interfaces
{
    /// <summary>
    /// Public interface for the Product repository
    /// </summary>
    public interface IProductRepository
    {
        /// <summary>
        /// Finds all products
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Product>> GetProducts();

        /// <summary>
        /// Finds product based on provided id
        /// </summary>
        /// <param name="productId">Product Id</param>
        /// <returns></returns>
        Task<Product> GetProduct(long productId);

        /// <summary>
        /// Updates product details 
        /// </summary>
        /// <param name="id">product id </param>
        /// <param name="productPatch">jsonPatch data with updates</param>
        /// <returns></returns>
        Task Update(long id, JsonPatchDocument productPatch);
    }
}