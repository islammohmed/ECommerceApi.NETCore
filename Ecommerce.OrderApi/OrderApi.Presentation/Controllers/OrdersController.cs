using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Application.DTOs;
using OrderApi.Application.DTOs.Conversions;
using OrderApi.Application.Interfaces;
using OrderApi.Application.Services;
namespace OrderApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController(IOrder orderInterface, IOrderService orderService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
        {
            var orders = await orderInterface.GetAllAsync();
            if (orders == null)
            {
                return NotFound("No Orders Found");

            }
            var (_, list) = OrderConversion.FromEntity(null, orders);
            return !list!.Any() ? NotFound() : Ok(list);
        }

        [HttpPost]
        public async Task<ActionResult<OrderDto>> CreateOrder(OrderDto orderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var order = OrderConversion.ToEntity(orderDto);
            var createdOrder = await orderInterface.GetAllAsync();
            if (createdOrder == null)
            {
                return BadRequest("Failed to create order");
            }
            var (_, list) = OrderConversion.FromEntity(null, createdOrder);
            return !list!.Any() ? NotFound() : Ok(list);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderDto>> GetOrderById(int id)
        {
            var order = await orderInterface.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound("Order not found");
            }
            var (orderDto, _) = OrderConversion.FromEntity(order, null);
            return orderDto is null ? NotFound() : Ok(orderDto);
        }
        [HttpPut]
        public async Task<ActionResult<OrderDto>> UpdateOrder(int id, OrderDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var order = OrderConversion.ToEntity(dto);
            var updatedOrder = await orderInterface.UpdateAsync(id, order);
            if (updatedOrder == null)
            {
                return NotFound("Failed to update order");
            }
            return updatedOrder.Flag ? Ok(updatedOrder) : BadRequest(updatedOrder);
        }
        [HttpDelete]
        public async Task<ActionResult<OrderDto>> DeleteOrder(int id)
        {
            var order = await orderInterface.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound("Order not found");
            }
            var deletedOrder = await orderInterface.DeleteAsync(id);
            if (deletedOrder == null)
            {
                return BadRequest("Failed to delete order");
            }
            return deletedOrder.Flag ? Ok(deletedOrder) : BadRequest(deletedOrder);
        }

        [HttpGet("client/{clientId:int}")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByClientId(int clientId)
        {
            var orders = await orderInterface.GetAllOrdersByClientId(clientId);
            if (orders == null)
            {
                return NotFound("No Orders Found");
            }
            var (_, list) = OrderConversion.FromEntity(null, orders);
            return !list!.Any() ? NotFound() : Ok(list);
        }
        [HttpGet("details/{id:int}")]
        public async Task<ActionResult<OrderDetailsDTO>> GetOrderDetails(int id)
        {
            var orderDetails = await orderService.GetOrderDetails(id);
            if (orderDetails == null)
            {
                return NotFound("Order details not found");
            }
            return Ok(orderDetails);
        }
    }

}