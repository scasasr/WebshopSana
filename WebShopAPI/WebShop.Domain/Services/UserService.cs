using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Domain.Models;

namespace WebShop.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly WebShopContext _context;

        public UserService(WebShopContext context)
        {
            _context = context;
        }
        public async Task<bool> IsUsernameExistsAsync(string username)
        {
            return await _context.Users.AnyAsync(u => u.UserName == username);
        }

        public async Task<User> LoginAsync(string email, string password)
        {
            try
            {
                return await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
            }
            catch (Exception ex) 
            {
                throw new Exception("Something went wrong during the user login. Error message: " + ex.Message);
            }
            
        }

        public async Task<User> RegisterAsync(User user)
        {
            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong during the user register. Error message: " + ex.Message);
            }
            
        }
    }
}
