using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update;
using AirsoftBattlefieldManagementSystemAPI.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftBattlefieldManagementSystemAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoomController(IRoomService roomService) : ControllerBase
    {
        [HttpGet("id/{id}")]
        public ActionResult<RoomDto> GetRoom(int id)
        {
            RoomDto roomDto = roomService.GetById(id);

            return Ok(roomDto);
        }

        [HttpGet("room-join-number/{roomJoinNumber}")]
        public ActionResult<RoomDto> GetRoomByRoomJoinNumber(int roomJoinNumber)
        {
            RoomDto roomDto = roomService.GetByRoomJoinNumber(roomJoinNumber);

            return Ok(roomDto);
        }

        [HttpPost("")]
        public ActionResult PostRoom([FromBody] CreateRoomDto roomDto)
        {
            int roomId = roomService.Create(roomDto);

            return Created($"/room/id/{roomId}", null);
        }

        [HttpPut("id/{id}")]
        public ActionResult PutRoom(int id, [FromBody] UpdateRoomDto roomDto)
        {
            roomService.Update(id, roomDto);

            return Ok();
        }

        [HttpDelete("id/{id}")]
        public ActionResult DeleteRoom(int id)
        {
            roomService.DeleteById(id);

            return NoContent();
        }
    }
}
