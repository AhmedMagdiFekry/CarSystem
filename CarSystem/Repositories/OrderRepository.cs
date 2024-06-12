using CarSystem.Data;
using CarSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CarSystem.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        void IOrderRepository.ApproveOrder(int orderId)
        {
           var order= _context.Orders.Find(orderId);
            if (order != null)
            {
                order.OrderStatusId = 2;// Approved
                _context.SaveChanges();
            }
        }

        IEnumerable<Order> IOrderRepository.GetApprovedOrders()
        {
           return _context.Orders.Where(p => p.OrderStatusId == 2)
                .Include(c=>c.Car)
                .Include(c=>c.AppUser)
                .ToList();
        }

        List<Order> IOrderRepository.GetOrdersByUserId(string userId)
        {
           return _context.Orders.Where(p=>p.UserId==userId).ToList();
        }

        IEnumerable<Order> IOrderRepository.GetPendingOrders()
        {
            return _context.Orders
            .Where(order => order.OrderStatusId == 1) // Pending status
            .Include(order => order.Car)
            .Include(order => order.AppUser)
            .ToList();
        }

        void IOrderRepository.RejectOrder(int orderId)
        {
            var order = _context.Orders.Find(orderId);
            if (order != null)
            {
                order.OrderStatusId = 3; // Rejected status
                _context.SaveChanges();
            }
        }
    }
}
