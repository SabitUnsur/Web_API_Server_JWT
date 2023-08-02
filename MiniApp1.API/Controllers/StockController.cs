using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MiniApp1.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        [Authorize(Policy ="AgePolicy")]
        [Authorize(Roles = "admin",Policy = "AnkaraPolicy")]
        [HttpGet]
        public IActionResult GetStock()
        {
            //Key --> type'a karşılık 
            //Value --> değere karşılık 
            var userName = HttpContext.User.Identity.Name;
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            //TokenService'da Id'yi bu claime belirttiğimiz için ClaimTypes.NameIdentifier olarak alındı.
            //Veritabanında userId,userName üzerinden gerekli datalar çekilir.
            return Ok($"Stock Operations => UserName: {userName} - UserId:{userIdClaim.Value}");
        }
    }
}
