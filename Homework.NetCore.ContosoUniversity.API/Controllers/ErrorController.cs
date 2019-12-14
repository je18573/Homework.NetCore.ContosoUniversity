using Microsoft.AspNetCore.Mvc;

namespace Homework.NetCore.ContosoUniversity.API.Controllers
{
    [ApiController]
    public class ErrorController : ControllerBase
    {
        // net core 3.0 只要寫這樣就好
        [Route("/error")]
        public IActionResult Error() => Problem();
    }
}