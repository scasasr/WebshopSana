using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Domain.Models;

namespace WebShop.Domain.Services
{
    public interface IOrderService
    {
        public Task<List<Order>> GetAllOrdersAsync();

        public Task<Order> GetOrderByIdAsync(int ordeID);

        public Task<List<Order>> GetOrdersByUserId(int userID);

        public Task<Order> AddOrderAsync(Order order);

        public Task<Order> UpdateOrderAsync(Order order);

        public Task<List<OrderProductRequested>> AddProductsToOrder(List<OrderProductRequested> orderProductsRequested);

    }
}
