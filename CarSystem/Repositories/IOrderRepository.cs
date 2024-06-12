using CarSystem.Models;

namespace CarSystem.Repositories
{
    public interface IOrderRepository :IRepository<Order>
    {
        List<Order> GetOrdersByUserId(string userId);
        IEnumerable<Order> GetPendingOrders();
        IEnumerable<Order> GetApprovedOrders();
        void ApproveOrder(int orderId);
        void RejectOrder(int orderId);
    }
}
