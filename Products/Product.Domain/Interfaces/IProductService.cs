using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;

namespace Product.Domain.Interfaces
{
    /// <summary>
    /// Service to manage product data
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// Finds all products
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Domain.Product>> GetProducts();
        
        /// <summary>
        /// Finds product based on provided Id
        /// </summary>
        /// <param name="productId">Product Id</param>
        /// <returns></returns>
        Task<Domain.Product> GetProduct(long productId);

        /// <summary>
        /// Update product details
        /// </summary>
        /// <param name="id">product id</param>
        /// <param name="productPatch">jsonPatch data with updates</param>
        /// <returns></returns>
        Task Update(long id, JsonPatchDocument productPatch);
    }
}