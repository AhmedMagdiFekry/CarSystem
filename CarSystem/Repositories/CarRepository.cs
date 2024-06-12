using CarSystem.Data;
using CarSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CarSystem.Repositories
{
    public class CarRepository : Repository<Car>, ICarRepository
    {
        private readonly ApplicationDbContext _context;

        public CarRepository(ApplicationDbContext context):base(context)
        {
            _context = context;
        }

        IEnumerable<Car> ICarRepository.GetApprovedCars()
        {
            return _context.Cars
           .Where(car => car.Orders.Any(order => order.OrderStatusId == 2)) // Approved status
           .Include(car => car.Category)
           .Include(car => car.AppUser)
           .ToList();
        }

        List<Car> ICarRepository.GetCarsByCategoryId(int categoryId)
        {
            return _context.Cars.Where(p=>p.CategoryId==categoryId).ToList();
        }

        IEnumerable<Car> ICarRepository.GetCarsByUserId(string userId)
        {
            return _context.Cars.Where(p=>p.UserId==userId).Include(c=>c.Category).ToList();
        }

    }
}
