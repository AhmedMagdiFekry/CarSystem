using CarSystem.Data;
using CarSystem.Models;

namespace CarSystem.Repositories
{
    public class CarRepository : Repository<Car>, ICarRepository
    {
        private readonly ApplicationDbContext _context;

        public CarRepository(ApplicationDbContext context):base(context)
        {
            _context = context;
        }

        List<Car> ICarRepository.GetCarsByCategoryId(int categoryId)
        {
            return _context.Cars.Where(p=>p.CategoryId==categoryId).ToList();
        }
    }
}
