using AirsoftBattlefieldManagementSystemAPI.Models.Dtos;
using AirsoftBattlefieldManagementSystemAPI.Services.Abstractions;
using AirsoftBattlefieldManagementSystemAPI.Services.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftBattlefieldManagementSystemAPI.Controllers
{
    [Controller]
    [Route("[controller]")]
    public class BattleController(IBattleService battleService) : Controller
    {
        [HttpGet]
        [Route("id/{id}")]
        public ActionResult<BattleDto> GetById(int id)
        {
            BattleDto? battleDto = battleService.GetById(id);

            if (battleDto is null) return NotFound();

            return Ok(battleDto);
        }

        [HttpPost]
        public ActionResult Create([FromBody] CreateBattleDto battleDto)
        {
            if (!ModelState.IsValid) return BadRequest();

            int battleId = battleService.Create(battleDto);

            return Created($"/Battle/id/{battleId}", null);
        }

        [HttpPut]
        [Route("id/{id}")]
        public ActionResult Update(int id, [FromBody] UpdateBattleDto battleDto)
        {
            if (!ModelState.IsValid) return BadRequest();

            bool isSuccessful = battleService.Update(id, battleDto);

            if (isSuccessful) return Ok();

            return NotFound();
        }

        [HttpDelete]
        [Route("id/{id}")]
        public ActionResult Delete(int id)
        {
            bool isSuccessful = battleService.DeleteById(id);

            if (isSuccessful) return NoContent();

            return NotFound();
        }
    }
}
