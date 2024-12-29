using System.Linq.Expressions;
using TopSpeed.Domain.Common;

namespace TopSpeed.Application.Contracts.Presistance
{
    public interface IGenericRepository<T> where T : BaseModel
    {
        Task<T> Create(T entity);

        Task<T> Delete(T entity);

        Task<List<T>> Get(Expression<Func<T, bool>> predicate);

        Task<List<T>> GetAllAsync();

        Task<T> GetByIdAsync(Guid id);

        IEnumerable<T> Query(Expression<Func<T,bool>> predicate);

        IEnumerable<T> Query();

        Task<bool> IsRecordExists(Expression<Func<T,bool>> predicate);

    }
}
