using AwesomePizzaBLL.Models;
using AwesomePizzaBLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AwesomePizzaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menu;

        public MenuController(IMenuService menu)
        {
            _menu = menu;
        }

        [HttpGet("products/{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _menu.GetProductByIdAsync(id);
            return Ok(product);
        }

        // Get all available product
        [HttpGet("products")]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _menu.GetProductsAsync();
            return Ok(products);
        }

        // Add a new product to the menu
        [HttpPost("products")]
        public async Task<IActionResult> AddProduct([FromBody] ProductModel product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newProduct = await _menu.AddProductAsync(product);

            return CreatedAtAction(nameof(GetProducts), new { id = newProduct.Id }, newProduct);
        }

        // Update a product in the menu
        //[HttpPatch("products/{id}")]
        //public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductModel product)
        //{
        //    if (id != product.Id || !ModelState.IsValid)
        //    {
        //        return BadRequest();
        //    }

        //    try
        //    {
        //        //
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ProductExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // Remove a product from the menu
        //[HttpDelete("products/{id}")]
        //public async Task<IActionResult> DeleteProduct(int id)
        //{


        //    return NoContent();
        //}
    }
}
