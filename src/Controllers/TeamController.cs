using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Team;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Zone;
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
        [Route("create")]
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
            teamService.Delete(id, User);

            return NoContent();
        }
        
        [HttpPost]
        [Route("leave")]
        public ActionResult Delete()
        {
            teamService.Leave(User);

            return Ok();
        }
        
        [HttpPost]
        [Route("spawn/teamId/{teamId}")]
        public ActionResult<TeamDto> CreateSpawn([FromBody] PostZoneDto zoneDto, int teamId)
        {
            TeamDto team = teamService.CreateSpawn(teamId, zoneDto, User);

            return Created($"/zone/id/{team.SpawnZoneId}", team);
        }
        
        [HttpDelete]
        [Route("spawn/teamId/{teamId}")]
        public ActionResult DeleteSpawn(int teamId)
        {
            teamService.DeleteSpawn(teamId, User);

            return NoContent();
        }
    }
}
