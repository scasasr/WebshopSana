using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Domain.Models;

namespace WebShop.Domain.Services
{
    public class OrderService : IOrderService
    {
        private readonly WebShopContext _context;

        public OrderService(WebShopContext context)
        {
            _context = context;
        }

        public async Task<Order> AddOrderAsync(Order order)
        {
            try
            {
                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();
                return order;
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong while adding the order. Error message: " + ex.Message);
            }
        }

        public async Task<List<OrderProductRequested>> AddProductsToOrder(List<OrderProductRequested> orderProductsRequested)
        {
            if (orderProductsRequested == null || orderProductsRequested.Count == 0)
            {
                throw new ArgumentException("The orderProductsRequested list cannot be null or empty.");
            }

            try
            {
                int orderId = orderProductsRequested.First().OrderId;
                var existingOrder = await _context.Orders.FirstOrDefaultAsync(or => or.OrderId == orderId) ;
                if (existingOrder == null)
                {
                    throw new Exception($"Order with ID {orderId} not found.");
                }

                foreach (var op in orderProductsRequested)
                {
                    var product = await _context.Products.FindAsync(op.ProductId);
                    if (product == null)
                    {
                        throw new Exception($"Product with ID {op.ProductId} not found.");
                    }

                    if (product.Stock - op.Quantity < 0)
                    {
                        throw new Exception($"Not enough stock for Product ID {op.ProductId}. Available: {product.Stock}, Requested: {op.Quantity}");
                    }

                    product.Stock -= op.Quantity;
                    _context.Products.Update(product);

                }

                await _context.OrderProductRequesteds.AddRangeAsync(orderProductsRequested);
                await _context.SaveChangesAsync();

                return orderProductsRequested;
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong while adding products to the order. Error: " + ex.Message);
            }
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            try
            {
                return await _context.Orders.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong during data orders load. Error message: " + ex.Message);
            }

        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            try 
            { 
                return await _context.Orders.FirstOrDefaultAsync(or => or.OrderId == orderId);
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong during data order load. Error message: " + ex.Message);
            }
        }

        public async Task<List<Order>> GetOrdersByUserId(int userID)
        {
            try
            {
                var orders= await _context.Orders.Where(or => or.UserId == userID).ToListAsync();
                return orders;
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong during data orders load. Error message: " + ex.Message);
            }
        }

        public async Task<Order> UpdateOrderAsync(Order order)
        {
            try
            {
                var existingOrder = await _context.Orders.FindAsync(order.OrderId);

                if (existingOrder == null) //Double check to verify if order exist
                {
                    throw new Exception("Order with ID not found.");
                }

                existingOrder.UserId = order.UserId;
                existingOrder.PayMethodId = order.PayMethodId;
                existingOrder.PaymentStatusId = order.PaymentStatusId;
                existingOrder.PaidDate = order.PaidDate;
                existingOrder.Total = order.Total;

                await _context.SaveChangesAsync();
                return existingOrder;
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong while updating the order. Error message: " + ex.Message);
            }
        }
    }
}
