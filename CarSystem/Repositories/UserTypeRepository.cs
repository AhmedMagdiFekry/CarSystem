using CarSystem.Data;
using CarSystem.Models;

namespace CarSystem.Repositories
{
    public class UserTypeRepository : Repository<UserType>, IUserTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public UserTypeRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        List<UserType> IUserTypeRepository.GetAdminOwnerUserTypes()
        {
           return _context.UserTypes.Where(p=>p.TypeName=="Admin"|| p.TypeName=="Owner").ToList();
        }
    }
}
