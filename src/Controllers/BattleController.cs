using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update;
using AirsoftBattlefieldManagementSystemAPI.Services.Abstractions;
using AirsoftBattlefieldManagementSystemAPI.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        public ActionResult Create([FromBody] PostBattleDto battleDto)
        {
            int battleId = battleService.Create(battleDto, User);

            return Created($"/Battle/id/{battleId}", null);
        }

        [HttpPut]
        [Route("id/{id}")]
        public ActionResult Update(int id, [FromBody] PutBattleDto battleDto)
        {
            battleService.Update(id, battleDto, User);

            return Ok();
        }

        [HttpDelete]
        [Route("id/{id}")]
        public ActionResult Delete(int id)
        {
            battleService.DeleteById(id, User);

            return NoContent();
        }
    }
}
