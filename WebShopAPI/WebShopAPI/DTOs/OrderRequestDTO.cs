namespace WebShopAPI.DTOs
{
    public class OrderRequestDTO
    {
        public int UserId { get; set; }

        public int PayMethodId { get; set; }

        public int PaymentStatusId { get; set; }

        public decimal Total { get; set; }
    }
}
