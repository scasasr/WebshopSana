using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebShop.Domain.Models;
using WebShop.Domain.Services;
using WebShopAPI.DTOs;

namespace WebShopAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductController(ILogger<ProductController> logger, IProductService productService, IMapper mapper)
        {
            _logger = logger;
            _productService = productService;
            _mapper = mapper;
        }


        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await _productService.GetAllActiveProductsAsync();
                return Ok(_mapper.Map<List<ProductResponseDTO>>(products));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all products.");
                return StatusCode(500, new { Message = "An unexpected error occurred. Please try again later." });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);

                if (product == null)
                {
                    return NotFound(new { Message = "Product not found." });
                }

                return Ok(_mapper.Map<ProductResponseDTO>(product));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the product with ID {Id}", id);
                return StatusCode(500, new { Message = "An unexpected error occurred. Please try again later." });
            }
        }

        [Authorize]
        [HttpGet("GetByOrder/{orderId}")]
        public async Task<IActionResult> GetProductsByOrder(int orderId)
        {
            try
            {
                var products = await _productService.GetProductsByOrder(orderId);

                if (products == null || products.Count == 0)
                {
                    return NotFound(new { Message = "No products found for this order." });
                }

                return Ok(_mapper.Map<List<ProductResponseDTO>>(products));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching products for Order ID {OrderId}", orderId);
                return StatusCode(500, new { Message = "An unexpected error occurred. Please try again later." });
            }
        }

        [HttpGet("GetPaginated")]
        public async Task<IActionResult> GetProductsPaginated([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (page <= 0 || pageSize <= 0)
            {
                return BadRequest(new { Message = "Page and pageSize must be greater than zero." });
            }

            try
            {
                var products = await _productService.GetProductsPaginatedAsync(page, pageSize);
                var items_ = await _productService.GetTotalProductCountAsync(); ;
                var totalPages_ = (int)Math.Ceiling(((double)items_ /pageSize));
                var response = new
                {
                    haveItems = products.Any(), 
                    items = items_, 
                    totalPages = totalPages_,
                    data = _mapper.Map<List<ProductResponseDTO>>(products) 
                };
                return Ok(response);
            }   
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching paginated products.");
                return StatusCode(500, new { Message = "An unexpected error occurred. Please try again later." });
            }
        }


    }
}
