using AuthServer.Core.Configuration;
using AuthServer.Core.Dtos;
using AuthServer.Core.Entity;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using SharedLibrary.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service.Services
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<UserApp> _userManager;

        private readonly CustomTokenOption _tokenOption;


        public TokenService(UserManager<UserApp> userManager,IOptions<CustomTokenOption> options)
        {
            _userManager = userManager;
            _tokenOption = options.Value;
        }


        private string CreateRefreshToken()
        {
            var numberByte = new Byte[32]; //kaç bytlelık bir string değer üreteceğiz.
            using var random = RandomNumberGenerator.Create();
            random.GetBytes(numberByte); //randomdan değerleri alıp numberbyte değerine aktardı.
            return Convert.ToBase64String(numberByte); 

            //32 bytelık random bir string ifade oluştu.
            //Bu class sadece bu sısnıf içerisinde kullanılacak.
        }

        private IEnumerable<Claim> GetClaims(UserApp userApp,List<String> audiences) 
            //kullanıcı ve hangi apiye istek gideceğiyle alakalı bilgiler tuttuk.
            //Token Payloadında bu bilgiler olacak.
        {
            var userList = new List<Claim> {
            new Claim(ClaimTypes.NameIdentifier,userApp.Id),
            new Claim(JwtRegisteredClaimNames.Email,userApp.Email),
            new Claim(ClaimTypes.Name,userApp.UserName),
            new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()) //random tokenID'si olsun.
            };

            userList.AddRange(audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));
            //Burada ise audiencestan gelecek yani apiler gelen tokenın .Aud() diyerek iznine bakacak ve eğer varsa claims olarak eklenecek. 

            return userList;
        }

        private IEnumerable<Claim> GetClaimsByClient(Client client)
        {
            var claims = new List<Claim>();
            claims.AddRange(client.Audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString());
            new Claim(JwtRegisteredClaimNames.Sub, client.Id.ToString()); //Bu token kim için oluşuyor bilgisi.

            return claims;
        }

        public TokenDto CreateToken(UserApp userApp)
        {
            var accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOption.AccessTokenExpiration); //access süresi
            var refreshTokenExpiration = DateTime.Now.AddMinutes(_tokenOption.RefreshTokenExpiration);
            var securityKey = SignService.GetSymmetricSecurityKey(_tokenOption.SecurityKey); //imzalanmıs token
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature.ToString()); //Şifrelendi.
            //SigningCredentials => Token oluşturulurken, istenirken bu credentials üstünden isteniyor.

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
              issuer: _tokenOption.Issuer,
              expires: accessTokenExpiration,
              notBefore: DateTime.Now,
              claims: GetClaims(userApp, _tokenOption.Audience),
              signingCredentials: signingCredentials);


            var handler = new JwtSecurityTokenHandler();
            var token = handler.WriteToken(jwtSecurityToken);

            var tokenDto = new TokenDto
            {
                AccessToken = token,
                RefreshToken = CreateRefreshToken(),
                AccesTokenExpiration = accessTokenExpiration,
                RefreshTokenExpiration = refreshTokenExpiration
            };

            return tokenDto;
        }

        public ClientTokenDto CreateTokenByClient(Client client)
        {
            var accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOption.AccessTokenExpiration);
            var securityKey = SignService.GetSymmetricSecurityKey(_tokenOption.SecurityKey); 
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature); 

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
              issuer: _tokenOption.Issuer,
              expires: accessTokenExpiration,
              notBefore: DateTime.Now,
              claims: GetClaimsByClient(client),
              signingCredentials: signingCredentials);


            var handler = new JwtSecurityTokenHandler();
            var token = handler.WriteToken(jwtSecurityToken);

            var clientTokenDto = new ClientTokenDto
            {
                AccessToken = token,       
                AccesTokenExpiration = accessTokenExpiration,
            };

            return clientTokenDto;
        }
    }
    
}
