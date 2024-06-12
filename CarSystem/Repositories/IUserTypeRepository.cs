using CarSystem.Models;

namespace CarSystem.Repositories
{
    public interface IUserTypeRepository: IRepository<UserType>
    {
        List<UserType> GetAdminOwnerUserTypes();
    }

}
