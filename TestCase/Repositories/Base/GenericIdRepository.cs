using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TestCase.Models;

namespace TestCase.Repositories.Base
{
    public class GenericIdRepository<T> : GenericRepository<T> where T : class
    {
        public AppData Conn => Context as AppData;

        public GenericIdRepository(DbContext context)
            : base(context)
        {
        }

        public override IQueryable<T> GetList(Expression<Func<T, bool>> filter = null)
        {
            var query = base.GetList(filter);
            return query;
        }

        public override async Task<bool> DeleteAsync(T item)
        {
            return await base.DeleteAsync(item);
        }
    }
}
