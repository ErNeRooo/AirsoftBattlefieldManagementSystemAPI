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

        [HttpDelete("id/{id}")]
        public ActionResult<string> Delete(int id)
        {
            playerService.DeleteById(id, User);

            return NoContent();
        }

    }
}
