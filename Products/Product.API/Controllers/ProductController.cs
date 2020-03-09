using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Product.Domain.Interfaces;

namespace Product.API.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll([FromServices] IProductService productService)
        {
            try
            {
                return Ok(await productService.GetProducts());
            }
            catch (Exception e)
            {
                return StatusCode(500, "Something was wrong");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Domain.Product>> GetById([FromServices] IProductService productService, long id)
        {
            try
            {
                return Ok(await productService.GetProduct(id));
            }
            catch (Exception e)
            {
                return StatusCode(500, "Something was wrong");
            }
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> UpdateDescription([FromServices]IProductService productService, [FromRoute]long id, [FromBody]JsonPatchDocument productPatch)
        {
            try
            {
                var allowedPath = new[] { $"/{nameof(Domain.Product.Description)}" };
                if (productPatch.Operations.Any(op => !allowedPath.Contains(op.path)))
                {
                    return BadRequest();
                }

                await productService.Update(id, productPatch);

                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, "Something was wrong");
            }
        }
    }
}
