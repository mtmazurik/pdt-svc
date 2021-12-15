using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace pdt_svc.Controllers
{
    [Route("[controller]")] //   /admin
    [ApiController]
    public class AdminController : ControllerBase
    {
        // GET:  /ping
        [HttpGet("ping")]
        public IActionResult Get()
        {
            return Ok("200 OK Ping");
        }
    }
}
