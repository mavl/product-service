using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Product.Domain.Exceptions;
using Product.Domain.Interfaces;

namespace Product.API.Controllers
{
    /// <summary>
    /// Product API
    /// </summary>
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Returns collection of products
        /// </summary>
        /// <param name="productService"></param>
        /// <returns></returns>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Domain.Product>),200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAll([FromServices] IProductService productService)
        {
            try
            {
                return Ok(await productService.GetProducts());
            }
            catch (Exception e)
            {
                _logger.LogError(e, string.Empty);
                return StatusCode(500, "Something was wrong");
            }
        }

        /// <summary>
        /// Returns specific product by id
        /// </summary>
        /// <param name="productService"></param>
        /// <param name="id">Product Id</param>
        /// <returns></returns>
        /// <response code="404">If product for provided id is not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Domain.Product), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Domain.Product>> GetById([FromServices] IProductService productService, long id)
        {
            try
            {
                return Ok(await productService.GetProduct(id));
            }
            catch (NotFoundException e)
            {
                _logger.LogError(e, string.Empty);
                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError(e, string.Empty);
                return StatusCode(500, "Something was wrong");
            }
        }

        /// <summary>
        /// Update description for provided product Id
        /// </summary>
        /// <remarks>
        /// sample request:
        /// [
        ///   {
        ///     "op":"replace",
        ///     "path": "/Description",
        ///     "value":"text"
        ///   }
        ///  ]
        /// </remarks>
        /// <param name="productService"></param>
        /// <param name="id">Product Id</param>
        /// <param name="productPatch">Product patch object - allowed only for Description attribute</param>
        /// <returns></returns>
        /// <response code="200">Update description succeeds returns no content</response>
        /// <response code="400">If input object is not valid or patch is trying to update different parameters then description</response>
        /// <response code="404">If product for provided id is not found</response>
        /// <response code="500">Internal server error</response>
        [HttpPatch("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> UpdateDescription([FromServices]IProductService productService, [FromRoute]long id, [FromBody]JsonPatchDocument productPatch)
        {
            try
            {
                var allowedPath = new[] { $"/{nameof(Domain.Product.Description)}" };
                if (productPatch == null || productPatch.Operations.Any(op => !allowedPath.Contains(op.path)))
                {
                    return BadRequest();
                }

                await productService.Update(id, productPatch);

                return Ok();
            }
            catch (NotFoundException e)
            {
                _logger.LogError(e, string.Empty);
                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError(e, string.Empty);
                return StatusCode(500,"Something was wrong");
            }
        }
    }
}
