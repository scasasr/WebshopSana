using WebShop.Domain.Models;
using WebShop.Domain.Services;
using WebShopAPI.DTOs;
using WebShopAPI.Helper;

namespace WebShopAPI.GraphQL.Mutations
{
    public class UserMutations
    {
        private readonly IUserService _userService;
        private readonly Common _common;

        public UserMutations(IUserService userService, Common common)
        {
            _userService = userService;
            _common = common;
        }

        public async Task<User> RegisterUser(RegisterUserDTO userDTO)
        {
            var user = new User
            {
                Name = userDTO.Name,
                UserName = userDTO.UserName,
                Email = userDTO.Email,
                Password = _common.encryptPassword(userDTO.Password),
                CreatedDate = DateTime.UtcNow,
                IsActive = true
            };

            return await _userService.RegisterAsync(user);
        }

        public async Task<string> LoginUser(string email, string password)
        {
            var encryptedPassword = _common.encryptPassword(password);
            var user = await _userService.LoginAsync(email, encryptedPassword);

            if (user == null) throw new Exception("Invalid credentials.");
            return _common.generateJWT(user);
        }
    }
}
