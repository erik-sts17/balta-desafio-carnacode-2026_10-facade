namespace Facade.Application.Coupon.Interfaces
{
    public interface ICouponService
    {
        public bool ValidateCoupon(string code);
        public decimal GetDiscount(string code);
        public void MarkCouponAsUsed(string code, string customerId);
    }
}