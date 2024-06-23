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
        public void MarkAsReserved(int carId)
        {
            var car = _context.Cars.Find(carId);
            if (car != null)
            {
                car.IsReserved = true;
                _context.SaveChanges();
            }
        }

        void ICarRepository.MarkAsReserved(int carId)
        {
            var car = _context.Cars.Find(carId);
            if (car != null)
            {
                car.IsReserved = true;
                _context.SaveChanges();
            }
        }

        void ICarRepository.DeleteCar(int carId)
        {
            var orders = _context.Orders.Where(o => o.CarId == carId).ToList();
            if (orders.Any())
            {
                _context.Orders.RemoveRange(orders);
            }

            var car = _context.Cars.Find(carId);
            if (car != null)
            {
                _context.Cars.Remove(car);
                _context.SaveChanges();
            }
        }

        Car ICarRepository.GetCarsOwnerById(int carId)
        {
            return _context.Cars
                .Include(p => p.Category)
                .Include(c => c.AppUser)
                .FirstOrDefault(f => f.Id == carId);
        }
    }
}
