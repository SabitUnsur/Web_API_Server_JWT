namespace AuthServer.Core.Configuration
{
    //Entity veya dto değil. Üyelikle alakalı olmayan apilere gideceği için bu sınıf tasarlandı.
    public class Client
    {
        public string Id { get; set; }
        public string ClientSecret { get; set; }

        public List<String> Audiences { get; set; } //Hangi Apiye erişeceği verisini tutmak için kullandık.
    }
}
