using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Repositories
{
    public interface IGenericRepository<T> where T : class , new()
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync(int id); //burada tek sorguda yapılacağı için veritabanına direkt gidebilir. Dolayısıyla IQueryable gerekli değil.
        IQueryable<T> Where(Expression<Func<T, bool>> expression); //Func<> delegesi T entity alan ve bool dönen metotu işaret eder.
        Task AddAync(T entity);
        void Remove(T entity);
        T Update(T entity);
        //_context.Entry(entity).State = EntityState.Modified; --> Ef Memorydeki durumuna bakıp gidip veritabanına kaydeder dolayısıyla aync metodu yoktur.

    }
}
