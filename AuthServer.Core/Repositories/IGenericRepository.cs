using System.Linq.Expressions;

namespace AuthServer.Core.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetByIdAsync(int id);
        Task<IEnumerable<TEntity>> GetAllAsync(); //burada tek sorguda yapılacağı için veritabanına direkt gidebilir. Dolayısıyla IQueryable gerekli değil.
        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression); //Func<> delegesi T entity alan ve bool dönen metotu işaret eder.
        Task AddAsync(TEntity entity);
        void Remove(TEntity entity);
        TEntity Update(TEntity entity);
        //_context.Entry(entity).State = EntityState.Modified; --> Ef Memorydeki durumuna bakıp gidip veritabanına kaydeder dolayısıyla aync metodu yoktur.

    }
}
