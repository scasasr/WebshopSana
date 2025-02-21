using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebShop.Domain.Models;

namespace WebShopAPI.Helper
{
    public class Common
    {
        private readonly IConfiguration _configuration;

        public Common(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string encryptPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Compute the hash of the password 
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convert byte array to a hexadecimal string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        public string generateJWT(User user)
        {
            // Get config key
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);

            // Create the user claims with custom names
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim("UserId", user.UserId.ToString()),          
                new Claim("username", user.UserName!),        
                new Claim("role", user.RoleId.ToString())                   
            };

            // Create the JWT token descriptor 
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                //Issuer = _configuration["Jwt:Issuer"],
                //Audience = _configuration["Jwt:Audience"]
            };

            // Generate the JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Return the serialized JWT as a string
            return tokenHandler.WriteToken(token);
        }
    }

}
