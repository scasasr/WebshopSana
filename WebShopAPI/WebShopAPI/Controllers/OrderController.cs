using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebShop.Domain.Models;
using WebShop.Domain.Services;
using WebShopAPI.DTOs;

namespace WebShopAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrderController(ILogger<OrderController> logger, IOrderService orderService, IMapper mapper) 
        {
            _logger = logger;
            _orderService = orderService;   
            _mapper = mapper;   
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                var orders = await _orderService.GetAllOrdersAsync();
                return Ok(_mapper.Map<List<OrderResponseDTO>>(orders));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all orders.");
                return StatusCode(500, new { Message = "An unexpected error occurred. Please try again later." });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(id);
                if (order == null)
                {
                    return NotFound(new { Message = "Order not found." });
                }

                return Ok(_mapper.Map<OrderResponseDTO>(order));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the order with ID {Id}", id);
                return StatusCode(500, new { Message = "An unexpected error occurred. Please try again later." });
            }
        }

        [HttpGet("GetByUser/{userId}")]
        public async Task<IActionResult> GetOrdersByUser(int userId)
        {
            try
            {
                var orders = await _orderService.GetOrdersByUserId(userId);
                if (orders == null || orders.Count == 0)
                {
                    return NotFound(new { Message = "No orders found for this user." });
                }

                return Ok(_mapper.Map<List<OrderResponseDTO>>(orders));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching orders for User ID {UserId}", userId);
                return StatusCode(500, new { Message = "An unexpected error occurred. Please try again later." });
            }
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddOrder([FromBody] OrderRequestDTO orderDto)
        {
            if (orderDto == null)
            {
                return BadRequest(new { Message = "Invalid order data." });
            }

            try
            {
                var order = _mapper.Map<Order>(orderDto);
                order.CreatedDate = DateTime.UtcNow;
                order.PaidDate = null;
                var createdOrder = await _orderService.AddOrderAsync(order);

                return Created(string.Empty, _mapper.Map<OrderResponseDTO>(createdOrder));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating an order.");
                return StatusCode(500, new { Message = "An unexpected error occurred. Please try again later." });
            }
        }

        [HttpPost("AddProductsToOrder/{orderId}")]
        public async Task<IActionResult> AddProductsToOrder(int orderId, [FromBody] List<OrderProductRequestedDTO> orderProductsDto)
        {
            if (orderProductsDto == null || orderProductsDto.Count == 0)
            {
                return BadRequest(new { Message = "The list of products cannot be empty." });
            }

            try
            {
                var orderProducts = orderProductsDto
                .Select(dto => {
                    var orderProduct = _mapper.Map<OrderProductRequested>(dto);
                    orderProduct.OrderId = orderId;
                    return orderProduct;
                })
                .ToList();
                var addedProducts = await _orderService.AddProductsToOrder(orderProducts);

                return Created(string.Empty, _mapper.Map<List<OrderProductResponseDTO>>(addedProducts));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding products to order ID {OrderId}", orderId);
                return StatusCode(500, new { Message = "An unexpected error occurred. Please try again later." });
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateOrder([FromBody] OrderUpdateDTO orderDto)
        {
            if (orderDto == null)
            {
                return BadRequest(new { Message = "Invalid order data." });
            }

            try
            {
                var existingOrder = await _orderService.GetOrderByIdAsync(orderDto.OrderId);
                if (existingOrder == null)
                {
                    return NotFound(new { Message = "Order not found." });
                }

                _mapper.Map(orderDto, existingOrder);
                var updatedOrder = await _orderService.UpdateOrderAsync(existingOrder);

                return Ok(_mapper.Map<OrderResponseDTO>(updatedOrder));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the order with ID {Id}", orderDto.OrderId);
                return StatusCode(500, new { Message = "An unexpected error occurred. Please try again later." });
            }
        }

       
    }
}
