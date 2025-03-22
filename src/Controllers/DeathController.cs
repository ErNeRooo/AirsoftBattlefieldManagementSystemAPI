using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Services.Abstractions;
using AirsoftBattlefieldManagementSystemAPI.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftBattlefieldManagementSystemAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class DeathController(IDeathService deathService) : Controller
    {
        [HttpGet]
        [Route("id/{id}")]
        public ActionResult<DeathDto> GetById(int id)
        {
            DeathDto deathDto = deathService.GetById(id);

            return Ok(deathDto);
        }

        [HttpGet]
        [Route("playerId/{playerId}")]
        public ActionResult<List<DeathDto>> GetDeathsOfPlayerWithId(int playerId)
        {
            List<DeathDto> deathDtos = deathService.GetAllOfPlayerWithId(playerId);

            return Ok(deathDtos);
        }

        [HttpPost]
        [Route("playerId/{playerId}")]
        public ActionResult Create(int playerId, [FromBody] PostDeathDto deathDto)
        {
            int id = deathService.Create(playerId, deathDto);

            return Created($"/Death/id/{id}", null);
        }

        [HttpPut]
        [Route("id/{id}")]
        public ActionResult Update(int id, [FromBody] PutDeathDto deathDto)
        {
            deathService.Update(id, deathDto);

            return Ok();
        }

        [HttpDelete]
        [Route("id/{id}")]
        public ActionResult Delete(int id)
        {
            deathService.DeleteById(id);

            return NoContent();
        }
    }
}
