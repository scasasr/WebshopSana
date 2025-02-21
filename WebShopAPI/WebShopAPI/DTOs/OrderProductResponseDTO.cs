namespace WebShopAPI.DTOs
{
    public class OrderProductResponseDTO
    {
        public int OrderProductRequestedId { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
