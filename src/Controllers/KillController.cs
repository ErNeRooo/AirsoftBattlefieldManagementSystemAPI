using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Kill;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Services.Implementations;
using AirsoftBattlefieldManagementSystemAPI.Services.KillService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftBattlefieldManagementSystemAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class KillController(IKillService killService) : Controller
    {
        [HttpGet]
        [Route("id/{id}")]
        public ActionResult<KillDto> GetById(int id)
        {
            KillDto killDto = killService.GetById(id, User);

            return Ok(killDto);
        }

        [HttpGet]
        [Route("playerId/{playerId}")]
        public ActionResult<List<KillDto>> GetKillsOfPlayerWithId(int playerId)
        {
            List<KillDto> killDtos = killService.GetAllOfPlayerWithId(playerId, User);

            return Ok(killDtos);
        }

        [HttpPost]
        [Route("playerId/{playerId}")]
        public ActionResult Create(int playerId, [FromBody] PostKillDto KillDto)
        {
            int id = killService.Create(playerId, KillDto, User);

            return Created($"/Kill/id/{id}", null);
        }

        [HttpPut]
        [Route("id/{id}")]
        public ActionResult Update(int id, [FromBody] PutKillDto killDto)
        {
            killService.Update(id, killDto, User);

            return Ok();
        }

        [HttpDelete]
        [Route("id/{id}")]
        public ActionResult Delete(int id)
        {
            killService.DeleteById(id, User);

            return NoContent();
        }
    }
}
