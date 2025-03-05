using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Services.Abstractions;
using AirsoftBattlefieldManagementSystemAPI.Services.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftBattlefieldManagementSystemAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DeathController(ILogger<DeathController> logger, IDeathService deathService) : Controller
    {
        [HttpGet]
        [Route("id/{id}")]
        public ActionResult<DeathDto> GetById(int id)
        {
            DeathDto? deathDto = deathService.GetById(id);

            if (deathDto is null) return NotFound();

            return Ok(deathDto);
        }

        [HttpGet]
        [Route("playerId/{playerId}")]
        public ActionResult<List<DeathDto>> GetDeathsOfPlayerWithId(int playerId)
        {
            List<DeathDto>? deathDtos = deathService.GetAllOfPlayerWithId(playerId);

            if (deathDtos is null) return NotFound();

            return Ok(deathDtos);
        }

        [HttpPost]
        [Route("playerId/{playerId}")]
        public ActionResult Create(int playerId, [FromBody] CreateDeathDto deathDto)
        {
            if(!ModelState.IsValid) return BadRequest();

            int? id = deathService.Create(playerId, deathDto);

            if(id is null) return NotFound();

            return Created($"/Death/id/{id}", null);
        }

        [HttpPut]
        [Route("id/{id}")]
        public ActionResult Update(int id, [FromBody] UpdateDeathDto deathDto)
        {
            if (!ModelState.IsValid) return BadRequest();

            bool isSuccessful = deathService.Update(id, deathDto);

            if (isSuccessful) return Ok();

            return NotFound();
        }

        [HttpDelete]
        [Route("id/{id}")]
        public ActionResult Delete(int id)
        {
            bool isSuccessful = deathService.DeleteById(id);

            if (isSuccessful) return NoContent();

            return NotFound();
        }
    }
}
