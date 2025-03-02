using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftBattlefieldManagementSystemAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TeamController(ILogger<TeamController> logger, ITeamService teamService) : Controller
    {
        [HttpGet]
        [Route("id/{id}")]
        public ActionResult<TeamDto> GetById(int id)
        {
            TeamDto? teamDto = teamService.GetById(id);

            if (teamDto is null) return NotFound();

            return Ok(teamDto);
        }

        [HttpPost]
        public ActionResult Create([FromBody] CreateTeamDto teamDto)
        {
            if(!ModelState.IsValid) return BadRequest();

            int teamId = teamService.Create(teamDto);

            return Created($"/team/id/{teamId}", null);
        }

        [HttpPut]
        [Route("id/{id}")]
        public ActionResult Update(int id, [FromBody] UpdateTeamDto teamDto)
        {
            if (!ModelState.IsValid) return BadRequest();

            bool isSuccessful = teamService.Update(id, teamDto);

            if (isSuccessful) return Ok();

            return NotFound();
        }

        [HttpDelete]
        [Route("id/{id}")]
        public ActionResult Delete(int id)
        {
            bool isSuccessful = teamService.DeleteById(id);

            if (isSuccessful) return NoContent();

            return NotFound();
        }
    }
}
