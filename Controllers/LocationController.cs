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
    public class LocationController(ILogger<LocationController> logger, ILocationService locationService) : Controller
    {
        [HttpGet]
        [Route("id/{id}")]
        public ActionResult<LocationDto> GetById(int id)
        {
            LocationDto? locationDto = locationService.GetById(id);

            if (locationDto is null) return NotFound();

            return Ok(locationDto);
        }

        [HttpGet]
        [Route("playerId/{playerId}")]
        public ActionResult<List<LocationDto>> GetLocationsOfPlayerWithId(int playerId)
        {
            List<LocationDto>? locationDtos = locationService.GetAllOfPlayerWithId(playerId);

            if (locationDtos is null) return NotFound();

            return Ok(locationDtos);
        }

        [HttpPost]
        [Route("playerId/{playerId}")]
        public ActionResult Create(int playerId, [FromBody] CreateLocationDto locationDto)
        {
            if(!ModelState.IsValid) return BadRequest();

            int? id = locationService.Create(playerId, locationDto);

            if(id is null) return NotFound();

            return Created($"/Location/id/{id}", null);
        }

        [HttpPut]
        [Route("id/{id}")]
        public ActionResult Update(int id, [FromBody] UpdateLocationDto locationDto)
        {
            if (!ModelState.IsValid) return BadRequest();

            bool isSuccessful = locationService.Update(id, locationDto);

            if (isSuccessful) return Ok();

            return NotFound();
        }

        [HttpDelete]
        [Route("id/{id}")]
        public ActionResult Delete(int id)
        {
            bool isSuccessful = locationService.DeleteById(id);

            if (isSuccessful) return NoContent();

            return NotFound();
        }
    }
}
