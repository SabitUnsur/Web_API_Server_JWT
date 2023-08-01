using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MiniApp2.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetInvoices()
        {
            var userName = HttpContext.User.Identity.Name;
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            //TokenService'da Id'yi bu claime belirttiğimiz için ClaimTypes.NameIdentifier olarak alındı.
            //Veritabanında userId,userName üzerinden gerekli datalar çekilir.
            return Ok($" Invoice Operations => UserName: {userName} - UserId:{userIdClaim.Value}");
        }
    }
}
