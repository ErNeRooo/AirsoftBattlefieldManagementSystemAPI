using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Death;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Services.DeathService;
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
            DeathDto deathDto = deathService.GetById(id, User);

            return Ok(deathDto);
        }

        [HttpGet]
        [Route("playerId/{playerId}")]
        public ActionResult<List<DeathDto>> GetDeathsOfPlayerWithId(int playerId)
        {
            List<DeathDto> deathDtos = deathService.GetAllOfPlayerWithId(playerId, User);

            return Ok(deathDtos);
        }

        [HttpPost]
        [Route("")]
        public ActionResult<DeathDto> Create([FromBody] PostDeathDto deathDto)
        {
            DeathDto resultDeath = deathService.Create(deathDto, User);

            return Created($"/Death/id/{resultDeath.DeathId}", resultDeath);
        }

        [HttpPut]
        [Route("id/{id}")]
        public ActionResult<DeathDto> Update(int id, [FromBody] PutDeathDto deathDto)
        {
            DeathDto resultDeath = deathService.Update(id, deathDto, User);

            return Ok(resultDeath);
        }

        [HttpDelete]
        [Route("id/{id}")]
        public ActionResult Delete(int id)
        {
            deathService.DeleteById(id, User);

            return NoContent();
        }
    }
}
