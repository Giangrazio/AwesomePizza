using AwesomePizzaAPI.Filters;
using AwesomePizzaBLL.Models;
using AwesomePizzaBLL.Services;
using AwesomePizzaDAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AwesomePizzaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // Create a new order
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderModel order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newOrder = await _orderService.CreateOrderAsync(order);

            return CreatedAtAction(nameof(GetOrder), new { id = newOrder.Id }, newOrder);
        }

        // Get all Orders
        [Authorize]
        [HttpGet()]
        public async Task<IActionResult> GetOrders()
        {
            List<OrderModel> orders = await _orderService.GetOrdersAsync();

            return Ok(orders);
        }

        // Get details of a specific order
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(long id)
        {
            var order = await _orderService.GetOrderAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        // Update the status of an order
        [Authorize]
        [HttpPatch("{id}/updateStatus")]
        public async Task<IActionResult> UpdateOrderStatus(long id, [FromBody] OrderStatus status)
        {
            try
            {
                await _orderService.UpdateOrderStatusAsync(id, status);
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return NotFound();
            }

            return NoContent();
        }

        // Get order status by unique order code
        [HttpGet("bycode/{orderCode}")]
        public async Task<IActionResult> GetOrderStatusByCode(string orderCode)
        {
            try
            {
                var order = await _orderService.GetOrderByCodeAsync(orderCode);
                if (order == null)
                {
                    return NotFound("Order not found.");
                }

                return Ok(order);
            }
            catch (Exception ex)
            {
                // Log the exception if necessary
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
