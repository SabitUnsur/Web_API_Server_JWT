namespace AuthServer.Core.Entity
{
    public class UserRefreshToken
    {
        public string UserId { get; set; } //Bu token kime ait?
        public string Code { get; set; } //RefreshToken'ın kendisi
        public DateTime Expiration { get; set; }
    }
}
