using Microsoft.AspNetCore.Mvc;

namespace AirsoftBattlefieldManagementSystemAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BmsController : ControllerBase
    {
        private readonly ILogger<BmsController> _logger;

        public BmsController(ILogger<BmsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult Get()
        {
            return Ok();
        }
    }
}
