using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Services
{
    //Mapleme işlemi Service katmanında yapılacağı için IServicede iki değer aldık.
    //Buradan direkt API'ye veri gideceği için artık Entityleri işlemek yerine direkt Dtolar döndürüldü.
    public interface IGenericService<TEntity,TDto> where TEntity : class  where TDto : class 
    {
        Task<Response<TDto>> GetByIdAsync(int id); 
        Task<Response<IEnumerable<TDto>>> GetAllAsync(int id);
        Task<Response<IEnumerable<TDto>>> Where(Expression<Func<TEntity, bool>> expression); 
        Task<Response<TDto>> AddAync(TEntity entity);
        Task<Response<NoDataDto>> Remove(TEntity entity);
        Task<Response<NoDataDto>> Update(TEntity entity);
    }
}
