using System.Linq.Expressions;

namespace Backend.Interfaces.Repositories;

public interface IRepository<TEntity> where TEntity : class
{
    IQueryable<TEntity> Query { get; }
    Task<TEntity?> FindByIdAsync(object[] id);
    Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>>? expression = null, bool tracked = true);
    void Add(TEntity entity);
    void Attach(TEntity entities);
    void Update(TEntity entity);
    void Remove(TEntity entity);
    Task<int> SaveChangeAsync();
}
