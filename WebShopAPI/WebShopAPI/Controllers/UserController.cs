using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebShop.Domain.Models;
using WebShop.Domain.Services;
using WebShopAPI.DTOs;
using WebShopAPI.Helper;

namespace WebShopAPI.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly Common _common;

        public UserController(ILogger<UserController> logger, IUserService userSerivce,IMapper mapper, Common common) 
        {
            _logger = logger;   
            _userService = userSerivce; 
            _mapper = mapper;
            _common = common;   
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterUserDTO userDTO)
        {
            if (string.IsNullOrWhiteSpace(userDTO.Name))
            {
                return BadRequest(new { Message = "The name cannot be empty or null." });
            }

            if (string.IsNullOrWhiteSpace(userDTO.UserName))
            {
                return BadRequest(new { Message = "The username cannot be empty or null." });
            }

            if (string.IsNullOrWhiteSpace(userDTO.Email))
            {
                return BadRequest(new { Message = "The email cannot be empty or null." });
            }

            if (string.IsNullOrWhiteSpace(userDTO.Password))
            {
                return BadRequest(new { Message = "The password cannot be empty or null." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _mapper.Map<User>(userDTO);
            user.Password = _common.encryptPassword(userDTO.Password);
            user.CreatedDate = DateTime.UtcNow;

            try
            {
                var registeredUser = await _userService.RegisterAsync(user);

                if (registeredUser == null)
                {
                    return BadRequest(new { Message = "Registration failed. Please try again." });
                }

                return CreatedAtAction(string.Empty, new { id = registeredUser.UserId }, new { Message = "User registered successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while registering the user.");
                return StatusCode(500, new { Message = "An unexpected error occurred. Please try again later." });
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUserDTO userDTO) 
        {
            try 
            {
                if (string.IsNullOrWhiteSpace(userDTO.Email) || string.IsNullOrWhiteSpace(userDTO.Password))
                {
                    return BadRequest(new { Message = "Email and password are required." });
                }

                var encryptedPassword = _common.encryptPassword(userDTO.Password!);
                var user = await _userService.LoginAsync(userDTO.Email!, encryptedPassword);

                if (user == null)
                {
                    return Unauthorized(new { Message = "Wrong credentials." });
                }

                var token = _common.generateJWT(user);

                if (string.IsNullOrWhiteSpace(token))
                {
                    return Unauthorized(new { Message = "wrong credentials" });
                }

                //return JWT
                return Ok(new { Token = token });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during login. User: {Email}", userDTO.Email);
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An unexpected error occurred. Please try again later.", Detail = ex.Message });
            }

        }

    }
}
