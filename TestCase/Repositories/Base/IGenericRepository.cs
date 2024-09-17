using System.Linq.Expressions;

namespace TestCase.Repositories.Base
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetList(Expression<Func<T, bool>> filter = null);

        Task<T> GetItemAsync(Guid id);

        Task<bool> CreateAsync(T item);

        Task<bool> EditAsync(T item);

        Task<bool> DeleteAsync(object id);

        Task<bool> DeleteAsync(T item);
    }
}
