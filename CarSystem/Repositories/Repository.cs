
using CarSystem.Data;

namespace CarSystem.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }
        void IRepository<T>.Add(T entity)
        {
            _context.Set<T>().Add(entity); 
            _context.SaveChanges();
        }

        void IRepository<T>.Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
        }

        List<T> IRepository<T>.GetAll()
        {
           return _context.Set<T>().ToList();
        }

        T IRepository<T>.GetById(int id)
        {
           return _context.Set<T>().Find(id);
        }

        void IRepository<T>.Update(T entity)
        {
           _context.Set<T>().Update(entity);
            _context.SaveChanges();
        }
    }
}
