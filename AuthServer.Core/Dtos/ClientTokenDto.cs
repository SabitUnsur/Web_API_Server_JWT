namespace AuthServer.Core.Dtos
{
    //RefreshToken olmayan yani üyelikle alakalı olmayan apilere gidecek istekler için yazılan Token
    public class ClientTokenDto
    {
        public string AccessToken { get; set; }
        public DateTime AccesTokenExpiration { get; set; }
    }
}
