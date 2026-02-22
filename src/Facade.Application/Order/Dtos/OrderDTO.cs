namespace Facade.Application.Order.Dtos
{
    public class OrderDTO
    {
        public string ProductId { get; set; } = string.Empty;   
        public int Quantity { get; set; }
        public string CustomerEmail { get; set; } = string.Empty;
        public string CreditCard { get; set; } = string.Empty;
        public string Cvv { get; set; } = string.Empty;
        public string ShippingAddress { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public string CouponCode { get; set; } = string.Empty;
        public decimal ProductPrice { get; set; }
    }
}