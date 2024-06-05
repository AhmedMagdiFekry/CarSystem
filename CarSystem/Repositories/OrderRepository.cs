using CarSystem.Data;
using CarSystem.Models;

namespace CarSystem.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        List<Order> IOrderRepository.GetOrdersByUserId(string userId)
        {
           return _context.Orders.Where(p=>p.UserId==userId).ToList();
        }
    }
}
