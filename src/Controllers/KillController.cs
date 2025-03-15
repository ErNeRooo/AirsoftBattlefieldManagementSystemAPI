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
    public class KillController(IKillService killService) : Controller
    {
        [HttpGet]
        [Route("id/{id}")]
        public ActionResult<KillDto> GetById(int id)
        {
            KillDto killDto = killService.GetById(id);

            return Ok(killDto);
        }

        [HttpGet]
        [Route("playerId/{playerId}")]
        public ActionResult<List<KillDto>> GetKillsOfPlayerWithId(int playerId)
        {
            List<KillDto> killDtos = killService.GetAllOfPlayerWithId(playerId);

            return Ok(killDtos);
        }

        [HttpPost]
        [Route("playerId/{playerId}")]
        public ActionResult Create(int playerId, [FromBody] CreateKillDto KillDto)
        {
            int id = killService.Create(playerId, KillDto);

            return Created($"/Kill/id/{id}", null);
        }

        [HttpPut]
        [Route("id/{id}")]
        public ActionResult Update(int id, [FromBody] UpdateKillDto killDto)
        {
            killService.Update(id, killDto);

            return Ok();
        }

        [HttpDelete]
        [Route("id/{id}")]
        public ActionResult Delete(int id)
        {
            killService.DeleteById(id);

            return NoContent();
        }
    }
}
