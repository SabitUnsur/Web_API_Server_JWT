using SharedLibrary.Dtos;
using System.Linq.Expressions;

namespace AuthServer.Core.Services
{
    //Mapleme işlemi Service katmanında yapılacağı için IServicede iki değer aldık.
    //Buradan direkt API'ye veri gideceği için artık Entityleri işlemek yerine direkt Dtolar döndürüldü.
    public interface IGenericService<TEntity, TDto> where TEntity : class where TDto : class
    {
        Task<Response<TDto>> GetByIdAsync(int id);
        Task<Response<IEnumerable<TDto>>> GetAllAsync();
        Task<Response<IEnumerable<TDto>>> Where(Expression<Func<TEntity, bool>> expression);
        Task<Response<TDto>> AddAsync(TDto entity);
        Task<Response<NoDataDto>> Remove(int id);
        Task<Response<NoDataDto>> Update(TDto entity, int id);
    }
}
