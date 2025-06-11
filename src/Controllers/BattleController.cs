using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Battle;
using AirsoftBattlefieldManagementSystemAPI.Services.BattleService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftBattlefieldManagementSystemAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class BattleController(IBattleService battleService) : Controller
    {
        [HttpGet]
        [Route("id/{id}")]
        public ActionResult<BattleDto> GetById(int id)
        {
            BattleDto battleDto = battleService.GetById(id, User);

            return Ok(battleDto);
        }

        [HttpPost]
        public ActionResult<BattleDto> Create([FromBody] PostBattleDto battleDto)
        {
            BattleDto battleResult = battleService.Create(battleDto, User);

            return Created($"/Battle/id/{battleResult.BattleId}", battleResult);
        }

        [HttpPut]
        [Route("id/{id}")]
        public ActionResult<BattleDto> Update(int id, [FromBody] PutBattleDto battleDto)
        {
            BattleDto battleResult = battleService.Update(id, battleDto, User);

            return Ok(battleResult);
        }

        [HttpDelete]
        [Route("id/{id}")]
        public ActionResult Delete(int id)
        {
            battleService.Delete(id, User);

            return NoContent();
        }
    }
}
