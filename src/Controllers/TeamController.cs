using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Team;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Services.TeamService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftBattlefieldManagementSystemAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class TeamController(ITeamService teamService) : Controller
    {
        [HttpGet]
        [Route("id/{id}")]
        public ActionResult<TeamDto> GetById(int id)
        {
            TeamDto teamDto = teamService.GetById(id, User);

            return Ok(teamDto);
        }

        [HttpPost]
        public ActionResult<TeamDto> Create([FromBody] PostTeamDto teamDto)
        {
            TeamDto resultTeam = teamService.Create(teamDto, User);

            return Created($"/team/id/{resultTeam.TeamId}", resultTeam);
        }

        [HttpPut]
        [Route("id/{id}")]
        public ActionResult<TeamDto> Update(int id, [FromBody] PutTeamDto teamDto)
        {
            TeamDto resultTeam = teamService.Update(id, teamDto, User);

            return Ok(resultTeam);
        }

        [HttpDelete]
        [Route("id/{id}")]
        public ActionResult Delete(int id)
        {
            teamService.DeleteById(id, User);

            return NoContent();
        }
    }
}
