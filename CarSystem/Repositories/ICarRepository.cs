using CarSystem.Models;

namespace CarSystem.Repositories
{
    public interface ICarRepository:IRepository<Car>
    {
        List<Car> GetCarsByCategoryId(int categoryId);
        IEnumerable<Car> GetCarsByUserId(string UserId);
        IEnumerable<Car> GetApprovedCars();
        void MarkAsReserved(int carId);
        void DeleteCar(int carId);
        Car GetCarsOwnerById(int carId);
    }
}
