using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TopSpeed.Application.Contracts.Presistance;
using TopSpeed.Domain.Common;
using TopSpeed.Infrastructure.Common;

namespace TopSpeed.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseModel
    {
        protected readonly ApplicationDbContext dbContext;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<T> Create(T entity)
        {
           await dbContext.AddAsync(entity);

            return entity;
        }

        public async Task<T> Delete(T entity)
        {
           dbContext.Set<T>().Remove(entity);

           await dbContext.SaveChangesAsync();

           return entity;
        }

        public async Task<List<T>> Get(Expression<Func<T, bool>> predicate)
        {
           return await dbContext.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<List<T>> GetAllAsync()
        {
           return await dbContext.Set<T>().AsNoTracking().ToListAsync(); 
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await dbContext.Set<T>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> IsRecordExists(Expression<Func<T, bool>> predicate)
        {
            return await dbContext.Set<T>().Where(predicate).AnyAsync();
        }

        public  IEnumerable<T> Query(Expression<Func<T, bool>> predicate)
        {
            var entities = dbContext.Set<T>().Where(predicate).ToList();

            return entities;
        }

        public IEnumerable<T> Query()
        {
            return dbContext.Set<T>().AsNoTracking().ToList();  
        }
    }
}
