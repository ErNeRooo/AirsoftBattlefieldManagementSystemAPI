using AirsoftBattlefieldManagementSystemAPI.Models.Dtos;
using AirsoftBattlefieldManagementSystemAPI.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftBattlefieldManagementSystemAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoomController(ILogger<RoomController> logger, IRoomService roomService) : ControllerBase
    {
        [HttpGet("id/{id}")]
        public ActionResult<RoomDto> GetRoom(int id)
        {
            RoomDto? roomDto = roomService.GetById(id);

            if (roomDto is null) return NotFound();

            return Ok(roomDto);
        }

        [HttpPost("")]
        public ActionResult PostRoom([FromBody] CreateRoomDto roomDto)
        {
            if (!ModelState.IsValid) return BadRequest();

            int roomId = roomService.Create(roomDto);

            return Created($"/room/id/{roomId}", null);
        }

        [HttpPut("id/{id}")]
        public ActionResult PutRoom(int id, [FromBody] UpdateRoomDto roomDto)
        {
            if (!ModelState.IsValid) return BadRequest();

            bool isSuccessful = roomService.Update(id, roomDto);

            if(isSuccessful) return Ok();

            return NotFound();
        }

        [HttpDelete("id/{id}")]
        public ActionResult DeleteRoom(int id)
        {
            bool isSuccessful = roomService.DeleteById(id);

            if (isSuccessful) return NoContent();

            return NotFound();
        }
    }
}
