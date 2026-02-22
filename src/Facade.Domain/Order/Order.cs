namespace Facade.Domain.Order
{
    public class Order
    {
        public string Id { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public string CouponCode { get; set; } = string.Empty;
    }
}