using Microsoft.AspNetCore.Identity;

namespace AuthServer.Core.Entity
{
    public class UserApp : IdentityUser
    {
        public string? City { get; set; }
    }
}
