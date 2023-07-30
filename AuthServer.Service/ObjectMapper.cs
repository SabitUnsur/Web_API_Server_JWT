using AutoMapper;

namespace AuthServer.Service
{
    // Automapper'ı sadece ilgili class library içerisinde kullanmak için ObjectMapper tanımladık.
    // Class library'lerde DI Container olmadığı için bu şekilde tanımladık.
    // _mapper.Map şeklinde yapmak yerine bu tasarımı yaptık.s
    public static class ObjectMapper
    {
        private static Lazy<IMapper> lazy = new Lazy<IMapper>(() =>  //Lazy loading yapıldı
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<DtoMapper>();
            });

            return config.CreateMapper();
        });

        public static IMapper Mapper => lazy.Value;
    }





}
