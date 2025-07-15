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
        public ActionResult<RoomWithRelatedEntitiesDto> GetById(int id)
        {
            RoomWithRelatedEntitiesDto roomDto = roomService.GetById(id);

            return Ok(roomDto);
        }

        [HttpGet("join-code/{joinCode}")]
        public ActionResult<RoomWithRelatedEntitiesDto> GetByJoinCode(string joinCode)
        {
            RoomWithRelatedEntitiesDto roomDto = roomService.GetByJoinCode(joinCode);

            return Ok(roomDto);
        }

        [HttpPost("")]
        public ActionResult<RoomDto> Create([FromBody] PostRoomDto postRoomDto)
        {
            RoomDto resultRoom = roomService.Create(postRoomDto, User);

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
        public ActionResult<RoomWithRelatedEntitiesDto> Join([FromBody] LoginRoomDto roomDto)
        {
            RoomWithRelatedEntitiesDto resultRoom = roomService.Join(roomDto, User);
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
