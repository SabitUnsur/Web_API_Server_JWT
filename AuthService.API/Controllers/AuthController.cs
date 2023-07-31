using AuthServer.Core.Dtos;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : CustomBaseController
    {
        private readonly IAuthenticationService _authService;

        public AuthController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateToken(LoginDto loginDto)
        {
            var result = await _authService.CreateTokenAsync(loginDto);
            return ActionResultInstance(result);
        }


        [HttpPost]
        public async Task<IActionResult> CreateTokenByClient(ClientLoginDto clientLoginDto)
        {
            var result = await _authService.CreateTokenByClient(clientLoginDto);
            return ActionResultInstance(result);
        }


        [HttpPost]
        public async Task<IActionResult> RevokeRefreshToken(RefreshTokenDto refreshTokenDto)
        {
            var result = await _authService.RevokeRefreshToken(refreshTokenDto.RefreshToken);
            return ActionResultInstance(result);
        }


        [HttpPost]
        public async Task<IActionResult> CreateTokenByRefreshToken(RefreshTokenDto refreshTokenDto)
        {
            var result = await _authService.CreateTokenByRefreshToken(refreshTokenDto.RefreshToken);
            return ActionResultInstance(result);
        }
    }
}
