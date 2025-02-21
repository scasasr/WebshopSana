namespace WebShopAPI.DTOs
{
    public class OrderResponseDTO
    {
        public int OrderId { get; set; }

        public int UserId { get; set; }

        public int PayMethodId { get; set; }

        public int PaymentStatusId { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? PaidDate { get; set; }

        public decimal Total { get; set; }
    }
}
