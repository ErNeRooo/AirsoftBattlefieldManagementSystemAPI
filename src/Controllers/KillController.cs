using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Kill;
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
        public ActionResult<KillDto> Create(int playerId, [FromBody] PostKillDto killDto)
        {
            KillDto resultKill = killService.Create(playerId, killDto, User);

            return Created($"/Kill/id/{resultKill.KillId}", resultKill);
        }

        [HttpPut]
        [Route("id/{id}")]
        public ActionResult<KillDto> Update(int id, [FromBody] PutKillDto killDto)
        {
            KillDto resultKill = killService.Update(id, killDto, User);

            return Ok(resultKill);
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
