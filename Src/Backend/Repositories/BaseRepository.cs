using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Backend.Interfaces.Repositories;

namespace Backend.Repositories
{
    public class BaseRepository<TEntity>(DbContext context) : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext context = context;
        private readonly IQueryable<TEntity> _query = context.Set<TEntity>();
        public virtual IQueryable<TEntity> Query => _query;
        public virtual void Add(TEntity entity)
        {
            context.Add(entity);
        }

        public virtual void Attach(TEntity entities)
        {
            context.Attach(entities);
        }

        public virtual async Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>>? expression = null, bool tracked = true)
        {
            var query = Query;
            if (expression != null)
                query = query.Where(expression);

            if (!tracked)
                query = query.AsNoTracking();

            return await query.ToListAsync();
        }

        public virtual async Task<TEntity?> FindByIdAsync(object[] id)
        {
            return await context.FindAsync<TEntity>(id);
        }

        public virtual void Remove(TEntity entity)
        {
            if (context.Entry(entity).State == EntityState.Detached)
            {
                Attach(entity);
            }
            context.Remove(entity);
        }

        public virtual void Update(TEntity entity)
        {
            Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }

        public async Task<int> SaveChangeAsync()
        {
            return await context.SaveChangesAsync();
        }
    }
}
