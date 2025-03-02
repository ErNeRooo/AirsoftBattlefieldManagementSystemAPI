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
    public class KillController(ILogger<KillController> logger, IKillService killService) : Controller
    {
        [HttpGet]
        [Route("id/{id}")]
        public ActionResult<KillDto> GetById(int id)
        {
            KillDto? KillDto = killService.GetById(id);

            if (KillDto is null) return NotFound();

            return Ok(KillDto);
        }

        [HttpGet]
        [Route("playerId/{playerId}")]
        public ActionResult<List<KillDto>> GetKillsOfPlayerWithId(int playerId)
        {
            List<KillDto>? KillDtos = killService.GetAllOfPlayerWithId(playerId);

            if (KillDtos is null) return NotFound();

            return Ok(KillDtos);
        }

        [HttpPost]
        [Route("playerId/{playerId}")]
        public ActionResult Create(int playerId, [FromBody] CreateKillDto KillDto)
        {
            if(!ModelState.IsValid) return BadRequest();

            int? id = killService.Create(playerId, KillDto);

            if(id is null) return NotFound();

            return Created($"/Kill/id/{id}", null);
        }

        [HttpPut]
        [Route("id/{id}")]
        public ActionResult Update(int id, [FromBody] UpdateKillDto killDto)
        {
            if (!ModelState.IsValid) return BadRequest();

            bool isSuccessful = killService.Update(id, killDto);

            if (isSuccessful) return Ok();

            return NotFound();
        }

        [HttpDelete]
        [Route("id/{id}")]
        public ActionResult Delete(int id)
        {
            bool isSuccessful = killService.DeleteById(id);

            if (isSuccessful) return NoContent();

            return NotFound();
        }
    }
}
