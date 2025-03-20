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
    public class TeamController(ITeamService teamService) : Controller
    {
        [HttpGet]
        [Route("id/{id}")]
        public ActionResult<TeamDto> GetById(int id)
        {
            TeamDto teamDto = teamService.GetById(id);

            return Ok(teamDto);
        }

        [HttpPost]
        public ActionResult Create([FromBody] PostTeamDto teamDto)
        {
            int teamId = teamService.Create(teamDto);

            return Created($"/team/id/{teamId}", null);
        }

        [HttpPut]
        [Route("id/{id}")]
        public ActionResult Update(int id, [FromBody] PutTeamDto teamDto)
        {
            teamService.Update(id, teamDto);

            return Ok();
        }

        [HttpDelete]
        [Route("id/{id}")]
        public ActionResult Delete(int id)
        {
            teamService.DeleteById(id);

            return NoContent();
        }
    }
}
