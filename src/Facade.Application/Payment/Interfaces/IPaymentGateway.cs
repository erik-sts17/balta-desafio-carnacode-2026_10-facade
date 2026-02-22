namespace Facade.Application.Payment.Interfaces
{
    public interface IPaymentGateway
    {
        public string InitializeTransaction(decimal amount);
        public bool ValidateCard(string cardNumber, string cvv);
        public bool ProcessPayment(string transactionId, string cardNumber);
        public void RollbackTransaction(string transactionId);
    }
}