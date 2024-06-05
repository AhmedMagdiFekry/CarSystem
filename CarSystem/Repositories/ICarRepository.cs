using CarSystem.Models;

namespace CarSystem.Repositories
{
    public interface ICarRepository:IRepository<Car>
    {
        List<Car> GetCarsByCategoryId(int categoryId);
    }
}
