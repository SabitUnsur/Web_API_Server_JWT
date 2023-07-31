using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Dtos;

namespace AuthServer.API.Controllers
{
    public class CustomBaseController : ControllerBase
    {
        //API içinde statuscode 200 ise Ok dön gibi bir işlemi sürekli yapmamak için tasarlandı.
        //Buradan direkt kod dönecek
        public IActionResult ActionResultInstance<T>(Response<T> response) where T : class
        {
            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode
            };
        }
    }
}
