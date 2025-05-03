using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Login;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Services.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftBattlefieldManagementSystemAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class PlayerController(IPlayerService playerService) : ControllerBase
    {
        [HttpGet("id/{id}")]
        public ActionResult<PlayerDto> GetPlayerById(int id)
        {
            PlayerDto playerDto = playerService.GetById(id, User);

            return Ok(playerDto);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public ActionResult<string> Register([FromBody] PostPlayerDto playerDto)
        {
            int playerId = playerService.Create(playerDto);
            string jwtToken = playerService.GenerateJwt(playerId);

            return Created($"/player/id/{playerId}", jwtToken);
        }

        [HttpPut("id/{id}")]
        public ActionResult<string> Update(int id, [FromBody] PutPlayerDto playerDto)
        {
            playerService.Update(id, playerDto, User);

            return Ok();
        }

        [HttpPatch("join-room")]
        public ActionResult<string> JoinRoom([FromBody] LoginRoomDto roomDto)
        {
            playerService.JoinRoom(roomDto, User);

            return Ok();
        }
        
        [HttpPatch("leave-room")]
        public ActionResult<string> LeaveRoom()
        {
            playerService.LeaveRoom(User);

            return Ok();
        }

        [HttpDelete("id/{id}")]
        public ActionResult<string> Delete(int id)
        {
            playerService.DeleteById(id, User);

            return NoContent();
        }

    }
}
