using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;

namespace Product.Domain.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Domain.Product>> GetProducts();
        
        Task<Domain.Product> GetProduct(long productId);
        
        Task Update(long id, JsonPatchDocument productPatch);
    }
}