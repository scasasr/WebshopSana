using WebShop.Domain.Models;

namespace WebShopAPI.DTOs
{
    public class ProductResponseDTO
    {
        public int ProductId { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public string ReferenceCode { get; set; } = null!;

        public decimal Price { get; set; }

        public int Stock { get; set; }

        public DateTime? CreatedDate { get; set; }

        public bool? IsActive { get; set; }

    }
}
