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

        IEnumerable<Order> IOrderRepository.GetOrdersByCarId(int carId)
        {
            return _context.Orders
           .Where(o => o.CarId == carId)
           .ToList();
        }

        List<Order> IOrderRepository.GetOrdersByUserId(string userId)
        {
            return _context.Orders
              .Include(o => o.Car)
              .Include(o => o.AppUser)
              .Where(o => o.UserId == userId)
              .ToList();
        }

        Order IOrderRepository.GetOrderWithDetails(int orderId)
        {
            return _context.Orders
                   .Include(o => o.Car)
                   .Include(o => o.AppUser)
                   .FirstOrDefault(o => o.Id == orderId);
        }

        IEnumerable<Order> IOrderRepository.GetPendingOrdersForAdmin()
        {


            return _context.Orders
                .Include(order => order.Car)
                .Include(order => order.AppUser)
                .Where(order => order.OrderStatusId == 1) // Orders created for admin approval (UserId == null for admin orders)
                .ToList();


        }

        public IEnumerable<Order> GetPendingOrdersForOwner(string ownerId)
        {
            return _context.Orders
         .Include(order => order.Car)
         .Include(order => order.AppUser)
         .Where(order => order.OrderStatusId == 1 && order.Car.UserId == ownerId) // Orders created by customers for owner review
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
