using CarSystem.Models;

namespace CarSystem.Repositories
{
    public interface IOrderRepository :IRepository<Order>
    {
        List<Order> GetOrdersByUserId(string userId);
        IEnumerable<Order> GetPendingOrdersForAdmin();
        IEnumerable<Order> GetPendingOrdersForOwner(string ownerId);
        IEnumerable<Order> GetApprovedOrders();
        void ApproveOrder(int orderId);
        void RejectOrder(int orderId);
        Order GetOrderWithDetails(int orderId);
        IEnumerable<Order> GetOrdersByCarId(int carId);
    }
}
