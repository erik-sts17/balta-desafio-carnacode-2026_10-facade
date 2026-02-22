namespace Facade.Application.Inventory.Interfaces
{
    public interface IInventoryService
    {
        public bool CheckAvailability(string productId);
        public void ReserveProduct(string productId, int quantity);
        public void ReleaseReservation(string productId, int quantity);
    }
}