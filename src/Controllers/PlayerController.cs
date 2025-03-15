using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Services.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftBattlefieldManagementSystemAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayerController(IPlayerService playerService) : ControllerBase
    {
        [HttpGet("id/{id}")]
        public ActionResult<PlayerDto> GetPlayerById(int id)
        {
            PlayerDto playerDto = playerService.GetById(id);

            return Ok(playerDto);
        }

        [HttpPost("")]
        public ActionResult<string> PostPlayer([FromBody] CreatePlayerDto playerDto)
        {
            int playerId = playerService.Create(playerDto);

            return Created($"/player/{playerId}", null);
        }

        [HttpPut("id/{id}")]
        public ActionResult<string> PutPlayer(int id, [FromBody] UpdatePlayerDto playerDto)
        {
            playerService.Update(id, playerDto);

            return Ok();
        }

        [HttpDelete("id/{id}")]
        public ActionResult<string> DeletePlayer(int id)
        {
            playerService.DeleteById(id);

            return NoContent();
        }

    }
}
