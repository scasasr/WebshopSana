using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Domain.Models;

namespace WebShop.Domain.Services
{
    public class ProductService : IProductService
    {
        private readonly WebShopContext _context;

        public ProductService(WebShopContext context)
        {
            _context = context;
        }
        public async Task<List<Product>> GetAllActiveProductsAsync()
        {
            try
            {
                return await _context.Products.Where(prod => prod.IsActive == true).ToListAsync();
            }
            catch (Exception ex) 
            {
                throw new Exception("Something went wrong during data products load. Error message: " + ex.Message);
            }
            
        }

        public async Task<Product> GetProductByIdAsync(int ProductId)
        {
            try
            {
                return await _context.Products.FirstOrDefaultAsync(prod => prod.ProductId == ProductId);
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong during data product load. Error message: " + ex.Message);
            }
        }

        public async Task<List<Product>> GetProductsByOrder(int OrderId)
        {
            try 
            { 
                var productsID = _context.OrderProductRequesteds.Where(oprod => oprod.OrderId == OrderId).Select(oprod => oprod.ProductId).ToList();
                var products = await _context.Products.Where(p => productsID.Contains(p.ProductId)).ToListAsync();

                return products;

            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong during data products load. Error message: " + ex.Message);
            }
        }

        public async Task<List<Product>> GetProductsPaginatedAsync(int page, int pageSize)
        {
            try
            {
                return await _context.Products
                .Where(prod => prod.IsActive == true)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong during paginated data products load. Error message: " + ex.Message);
            }
        }

        public async Task<int> GetTotalProductCountAsync()
        {
            try 
            { 
                return await _context.Products.Where(prod => prod.IsActive == true).CountAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong while getting value count data products. Error message: " + ex.Message);
            }
        }
    }
}
