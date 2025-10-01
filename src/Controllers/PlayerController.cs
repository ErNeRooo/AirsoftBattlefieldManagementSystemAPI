using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Player;
using AirsoftBattlefieldManagementSystemAPI.Services.PlayerService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftBattlefieldManagementSystemAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class PlayerController(IPlayerService playerService) : ControllerBase
    {
        [HttpGet("me")]
        public ActionResult<PlayerDto> GetMe()
        {
            PlayerDto playerDto = playerService.GetMe(User);

            return Ok(playerDto);
        }
        
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
            PlayerDto resultPlayer = playerService.Create(playerDto);
            string jwtToken = playerService.GenerateJwt(resultPlayer.PlayerId);

            return Created($"/player/id/{resultPlayer.PlayerId}", jwtToken);
        }
        
        [HttpPost("kick-from-room/playerId/{playerId}")]
        public ActionResult<PlayerDto> KickFromRoom(int playerId)
        {
            PlayerDto playerDto = playerService.KickFromRoom(playerId, User);

            return Ok(playerDto);
        }
        
        [HttpPost("kick-from-team/playerId/{playerId}")]
        public ActionResult<PlayerDto> KickFromTeam(int playerId)
        {
            PlayerDto playerDto = playerService.KickFromTeam(playerId, User);

            return Ok(playerDto);
        }

        [HttpPut("")]
        public ActionResult<PlayerDto> Update([FromBody] PutPlayerDto playerDto)
        {
            PlayerDto player = playerService.Update(playerDto, User);

            return Ok(player);
        }

        [HttpDelete("")]
        public ActionResult<string> Delete()
        {
            playerService.Delete(User);

            return NoContent();
        }

    }
}
