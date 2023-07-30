using AuthServer.Core.Dtos;
using SharedLibrary.Dtos;

namespace AuthServer.Core.Services
{
    public interface IAuthenticationService
    {
        Task<Response<TokenDto>> CreateTokenAsync(LoginDto loginDto);
        Task<Response<TokenDto>> CreateTokenByRefreshToken(string refreshToken);
        Task<Response<NoDataDto>> RevokeRefreshToken(string refreshToken); //kullanıcı logOut olduğunda tokenın etkinliğini kırmak için.
        Task<Response<ClientTokenDto>> CreateTokenByClient(ClientLoginDto clientLoginDto);

    }
}
