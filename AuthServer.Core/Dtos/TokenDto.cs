namespace AuthServer.Core.Dtos
{
    public class TokenDto
    {
        public string AccessToken { get; set; }
        public DateTime AccesTokenExpiration { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
    }
}
