using AirsoftBattlefieldManagementSystemAPI.Models.Dtos;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftBattlefieldManagementSystemAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayerController(ILogger<PlayerController> logger, IPlayerService playerService) : ControllerBase
    {
        [HttpGet("id/{id}")]
        public ActionResult<PlayerDto> GetPlayerById(int id)
        {
            PlayerDto playerDto = playerService.GetById(id);

            if (playerDto == null) return NotFound("Player not found");

            return Ok(playerDto);
        }

        [HttpPost("")]
        public ActionResult<string> PostPlayer([FromBody] CreatePlayerDto playerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int playerId = playerService.Create(playerDto);

            return Created($"/player/{playerId}", null);
        }

        [HttpPut("id/{id}")]
        public ActionResult<string> PutPlayer(int id, [FromBody] UpdatePlayerDto playerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool isUpdateSuccessful = playerService.Update(id, playerDto);

            if (isUpdateSuccessful) return Ok();

            return NotFound();
        }

        [HttpDelete("id/{id}")]
        public ActionResult<string> DeletePlayer(int id)
        {
            bool isDeleteSuccessful = playerService.DeleteById(id);

            if (isDeleteSuccessful) return NoContent();

            return NotFound();
        }

    }
}
