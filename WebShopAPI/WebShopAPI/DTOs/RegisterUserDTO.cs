namespace WebShopAPI.DTOs
{
    public class RegisterUserDTO
    {
        public string Name { get; set; }

        public string Identification { get; set; } 

        public string UserName { get; set; } = null!;

        public string Email { get; set; } 

        public string Password { get; set; } 

        public string? PhoneNumber { get; set; }

        public int RoleId { get; set; } = 2;

        public bool? IsActive { get; set; } = true;
    }
}
