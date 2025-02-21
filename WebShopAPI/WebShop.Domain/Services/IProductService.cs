using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Domain.Models;

namespace WebShop.Domain.Services
{
    public interface IProductService
    {
        public Task<List<Product>> GetAllActiveProductsAsync();

        public Task<List<Product>> GetProductsPaginatedAsync(int page, int pageSize);//Get active product paginated

        public Task<Product> GetProductByIdAsync(int ProductId);

        public Task<List<Product>> GetProductsByOrder(int OrderId);

        public Task<int> GetTotalProductCountAsync();
    }
}
