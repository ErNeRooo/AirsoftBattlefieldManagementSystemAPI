using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Room;
using AirsoftBattlefieldManagementSystemAPI.Services.RoomService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftBattlefieldManagementSystemAPI.Controllers
{

    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class RoomController(IRoomService roomService) : ControllerBase
    {
        [HttpGet("id/{id}")]
        public ActionResult<RoomDto> GetById(int id)
        {
            RoomDto roomDto = roomService.GetById(id);

            return Ok(roomDto);
        }

        [HttpGet("join-code/{joinCode}")]
        public ActionResult<RoomDto> GetByJoinCode(string joinCode)
        {
            RoomDto roomDto = roomService.GetByJoinCode(joinCode);

            return Ok(roomDto);
        }

        [HttpPost("")]
        public ActionResult<RoomDto> Create([FromBody] PostRoomDto roomDto)
        {
            RoomDto resultRoom = roomService.Create(roomDto, User);

            return Created($"/room/id/{resultRoom.RoomId}", resultRoom);
        }

        [HttpPut("")]
        public ActionResult<RoomDto> Update([FromBody] PutRoomDto roomDto)
        {
            RoomDto resultRoom = roomService.Update(roomDto, User);

            return Ok(resultRoom);
        }

        [HttpDelete("")]
        public ActionResult Delete()
        {
            roomService.Delete(User);

            return NoContent();
        }

        [HttpPost("join")]
        public ActionResult<RoomDto> Join([FromBody] LoginRoomDto roomDto)
        {
            RoomDto resultRoom = roomService.Join(roomDto, User);
            return Ok(resultRoom);
        }

        [HttpPost("leave")]
        public ActionResult<string> Leave()
        {
            roomService.Leave(User);

            return Ok();
        }
    }
}
