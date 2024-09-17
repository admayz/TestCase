using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace TestCase.Repositories.Base
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected DbContext Context;

        protected DbSet<T> DbSet;

        public GenericRepository(DbContext context)
        {
            Context = context;

            if (context != null)
                DbSet = context.Set<T>();
        }

        #region GetList

        public virtual IQueryable<T> GetList(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> queryable = DbSet.AsNoTracking();
            if (filter != null)
            {
                queryable = queryable.Where(filter);
            }

            return queryable;
        }

        #endregion

        #region GetItem

        public virtual async Task<T> GetItemAsync(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        #endregion

        #region Create

        public virtual async Task<bool> CreateAsync(T item)
        {
            var idProperty = typeof(T).GetProperty("Id");
            if (idProperty != null && idProperty.PropertyType == typeof(Guid))
                idProperty.SetValue(item, Guid.NewGuid());

            DbSet.Add(item);
            await Context.SaveChangesAsync();
            return true;
        }

        #endregion

        #region Edit

        public virtual async Task<bool> EditAsync(T item)
        {
            Context.Entry(item).State = EntityState.Modified;
            await Context.SaveChangesAsync();
            return true;
        }

        #endregion

        #region Delete

        public virtual async Task<bool> DeleteAsync(object id)
        {
            return await DeleteAsync(await DbSet.FindAsync(id));
        }

        public virtual async Task<bool> DeleteAsync(T item)
        {
            if (Context.Entry(item).State == EntityState.Detached)
            {
                DbSet.Attach(item);
            }

            DbSet.Remove(item);
            await Context.SaveChangesAsync();
            return true;
        }

        #endregion
    }
}
