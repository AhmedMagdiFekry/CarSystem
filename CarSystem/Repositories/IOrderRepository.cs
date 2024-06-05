using CarSystem.Models;

namespace CarSystem.Repositories
{
    public interface IOrderRepository :IRepository<Order>
    {
        List<Order> GetOrdersByUserId(string userId);
    }
}
