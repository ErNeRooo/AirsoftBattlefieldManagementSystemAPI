using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.MapPing;
using AirsoftBattlefieldManagementSystemAPI.Services.MapPingService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftBattlefieldManagementSystemAPI.Controllers
{
    [ApiController]
    [Route("map-ping")]
    [Authorize]
    public class MapPingController(IMapPingService mapPingService) : Controller
    {
        [HttpGet]
        [Route("id/{id}")]
        public ActionResult<MapPingDto> GetById(int id)
        {
            MapPingDto mapPingDto = mapPingService.GetById(id, User);

            return Ok(mapPingDto);
        }
        
        [HttpGet]
        [Route("teamId/{teamId}")]
        public ActionResult<MapPingDto> GetAllOfTeamWithId(int teamId)
        {
            List<MapPingDto> mapPingDtos = mapPingService.GetAllOfTeamWithId(teamId, User);

            return Ok(mapPingDtos);
        }

        [HttpPost]
        [Route("")]
        public ActionResult<MapPingDto> Create([FromBody] PostMapPingDto mapPingDto)
        {
            MapPingDto resultMapPing = mapPingService.Create(mapPingDto, User);

            return Created($"/MapPing/id/{resultMapPing.MapPingId}", resultMapPing);
        }

        [HttpDelete]
        [Route("id/{id}")]
        public ActionResult Delete(int id)
        {
            mapPingService.DeleteById(id, User);

            return NoContent();
        }
    }
}
