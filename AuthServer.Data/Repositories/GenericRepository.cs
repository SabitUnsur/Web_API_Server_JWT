using AuthServer.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Data.Repossitories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class , new()
    {
        private readonly AppDbContext _appDbContext;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            _dbSet = appDbContext.Set<TEntity>();
        }

        public async Task AddAsync(TEntity entity)
        {
           await _dbSet.AddAsync(entity);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync(); //Tüm data cekildikten sonra üzerinde islem yapar.
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if(entity != null){
                _appDbContext.Entry(entity).State = EntityState.Detached; //Memoryde izlenmesin diye yazıldı.
            }
            return entity;
        }

        public void Remove(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public TEntity Update(TEntity entity)
        {
            //_appDbContext.Entry(entity).State = EntityState.Modified; //Propertyde tek bir alan değiştirilse dahi tüm alanlar güncellenecek gibi işlem yapar, dolayısıyla dezavantajdır.
           _appDbContext.Update(entity);
            return entity;
        }

        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression)
        {
            return _dbSet.Where(expression); //Önce filtreleme sonra veritabanından çekme yapar. ToList() dendiğidne çeker. 
        }
    }
}
