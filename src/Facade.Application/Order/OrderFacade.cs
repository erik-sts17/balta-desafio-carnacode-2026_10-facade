using Facade.Application.Coupon;
using Facade.Application.Inventory;
using Facade.Application.Order.Dtos;
using Facade.Application.Payment;
using Facade.Application.Shipping;

namespace Facade.Application.Order
{
    public class OrderFacade(InventoryService inventory, 
                             PaymentGateway payment, 
                             CouponService coupon, 
                             ShippingService shipping)
    {
        private readonly InventoryService _inventory = inventory;
        private readonly PaymentGateway _payment = payment;
        private readonly CouponService _coupon = coupon;
        private readonly ShippingService _shipping = shipping;

        public bool PlaceOrder(OrderDTO order)
        {
            Console.WriteLine("=== Processando Pedido (Via Facade) ===\n");

            try
            {
                // 1 - Estoque
                if (!_inventory.CheckAvailability(order.ProductId))
                {
                    Console.WriteLine("Produto indisponível");
                    return false;
                }

                _inventory.ReserveProduct(order.ProductId, order.Quantity);

                // 2 - Cupom
                decimal discount = 0;
                if (!string.IsNullOrEmpty(order.CouponCode) &&
                    _coupon.ValidateCoupon(order.CouponCode))
                {
                    discount = _coupon.GetDiscount(order.CouponCode);
                }

                // 3 - Cálculo
                decimal subtotal = order.ProductPrice * order.Quantity;
                decimal discountAmount = subtotal * discount;
                decimal shippingCost = _shipping.CalculateShipping(order.ZipCode, order.Quantity * 0.5m);
                decimal total = subtotal - discountAmount + shippingCost;

                // 4 - Pagamento
                string transactionId = _payment.InitializeTransaction(total);

                if (!_payment.ValidateCard(order.CreditCard, order.Cvv))
                {
                    _inventory.ReleaseReservation(order.ProductId, order.Quantity);
                    Console.WriteLine("Cartão inválido");
                    return false;
                }

                if (!_payment.ProcessPayment(transactionId, order.CreditCard))
                {
                    _inventory.ReleaseReservation(order.ProductId, order.Quantity);
                    _payment.RollbackTransaction(transactionId);
                    Console.WriteLine("Pagamento recusado");
                    return false;
                }

                // 5 - Envio
                string orderId = $"ORD{DateTime.Now.Ticks}";
                string labelId = _shipping.CreateShippingLabel(orderId, order.ShippingAddress);
                _shipping.SchedulePickup(labelId, DateTime.Now.AddDays(1));

                // 6 - Cupom usado
                if (!string.IsNullOrEmpty(order.CouponCode))
                    _coupon.MarkCouponAsUsed(order.CouponCode, order.CustomerEmail);

                Console.WriteLine($"\nPedido {orderId} finalizado com sucesso!");
                Console.WriteLine($"Total: R$ {total:N2}");

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao processar pedido: {ex.Message}");
                return false;
            }
        }
    }
}