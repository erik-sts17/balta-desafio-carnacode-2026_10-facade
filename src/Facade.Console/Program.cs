using Facade.Application.Coupon;
using Facade.Application.Inventory;
using Facade.Application.Order;
using Facade.Application.Order.Dtos;
using Facade.Application.Payment;
using Facade.Application.Shipping;

var facade = new OrderFacade(
      new InventoryService(),
      new PaymentGateway(),
      new CouponService(),
      new ShippingService()
  );

var order = new OrderDTO
{
    ProductId = "PROD001",
    Quantity = 2,
    CustomerEmail = "cliente@email.com",
    CreditCard = "1234567890123456",
    Cvv = "123",
    ShippingAddress = "Rua Exemplo, 123",
    ZipCode = "12345-678",
    CouponCode = "PROMO10",
    ProductPrice = 100.00m
};

facade.PlaceOrder(order);