namespace WebShopAPI.DTOs
{
    public class OrderProductRequestedDTO
    {
        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

    }
}
