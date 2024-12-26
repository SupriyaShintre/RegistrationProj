using Microsoft.AspNetCore.Mvc;

namespace Registration.Controllers
{
    public class LoginController : Controller
    {
        
        [HttpGet]
        [Route("[action]")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
