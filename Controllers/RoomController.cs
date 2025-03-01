using Microsoft.AspNetCore.Mvc;

namespace AirsoftBattlefieldManagementSystemAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly ILogger<RoomController> _logger;

        public RoomController(ILogger<RoomController> logger)
        {
            _logger = logger;
        }

        [HttpGet("id/{id}")]
        public ActionResult<string> GetRoom(int id)
        {
            return Ok($"Room {id}");
        }

        [HttpPost("id/{id}")]
        public ActionResult<string> PostRoom(int id)
        {
            return Created();
        }

        [HttpPut("id/{id}")]
        public ActionResult<string> PutRoom(int id)
        {
            return Ok($"put {id}");
        }

        [HttpDelete("id/{id}")]
        public ActionResult<string> DeleteRoom(int id)
        {
            return NoContent();
        }
    }
}
