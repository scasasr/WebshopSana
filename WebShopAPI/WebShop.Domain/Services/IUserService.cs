using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Domain.Models;

namespace WebShop.Domain.Services
{
    public interface IUserService
    {
        Task<bool> IsUsernameExistsAsync(string username); 
        Task<User> RegisterAsync(User user);  
        Task<User> LoginAsync(string username, string password);
    }
}
