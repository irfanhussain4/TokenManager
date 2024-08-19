using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AngularAuthentication.APICore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        [Route("CheckHealth")]
        public string CheckHealth()
        {
            return "I'm good, how are you?";
        }
    }
}
