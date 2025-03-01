using Microsoft.AspNetCore.Mvc;

namespace AirsoftBattlefieldManagementSystemAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly ILogger<PlayerController> _logger;

        public PlayerController(ILogger<PlayerController> logger)
        {
            _logger = logger;
        }

        [HttpGet("id/{id}")]
        public ActionResult<string> GetPlayer(int id)
        {
            return Ok($"player {id}");
        }

        [HttpPost("id/{id}")]
        public ActionResult<string> PostPlayer(int id)
        {
            return Created();
        }

        [HttpPut("id/{id}")]
        public ActionResult<string> PutPlayer(int id)
        {
            return Ok($"put {id}");
        }

        [HttpDelete("id/{id}")]
        public ActionResult<string> DeletePlayer(int id)
        {
            return NoContent();
        }

    }
}
