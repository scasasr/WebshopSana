using WebShop.Domain.Models;
using WebShop.Domain.Services;

namespace WebShopAPI.GraphQL.Queries
{
    public class ProductQuery
    {
        private readonly IProductService _productService;

        public ProductQuery(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<List<Product>> GetProducts() => await _productService.GetAllActiveProductsAsync();

        public async Task<Product> GetProductById(int id) => await _productService.GetProductByIdAsync(id);

        public async Task<ProductsPaginationResponse> GetProductsPaginated(int page, int pageSize)
        {
            var products = await _productService.GetProductsPaginatedAsync(page, pageSize);
            var totalItems = await _productService.GetTotalProductCountAsync();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            return new ProductsPaginationResponse
            {
                HaveItems = products.Any(),
                Items = totalItems,
                TotalPages= totalPages,
                Data = products
            };
        }

        public async Task<List<Product>> GetProductsByOrder(int orderId)
        {
            var products = await _productService.GetProductsByOrder(orderId);
            if (products == null || !products.Any())
            {
                throw new Exception($"No products found for Order ID {orderId}.");
            }

            return products;
        }
    }
}

