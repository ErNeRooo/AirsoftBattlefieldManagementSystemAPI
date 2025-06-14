﻿using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Location;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Services.LocationService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftBattlefieldManagementSystemAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class LocationController(ILocationService locationService) : Controller
    {
        [HttpGet]
        [Route("id/{id}")]
        public ActionResult<LocationDto> GetById(int id)
        {
            LocationDto locationDto = locationService.GetById(id, User);

            return Ok(locationDto);
        }

        [HttpGet]
        [Route("playerId/{playerId}")]
        public ActionResult<List<LocationDto>> GetLocationsOfPlayerWithId(int playerId)
        {
            List<LocationDto> locationDtos = locationService.GetAllLocationsOfPlayerWithId(playerId, User);

            return Ok(locationDtos);
        }

        [HttpPost]
        [Route("")]
        public ActionResult Create([FromBody] PostLocationDto locationDto)
        {
            LocationDto resultLocation = locationService.Create(locationDto, User);

            return Created($"/Location/id/{resultLocation.LocationId}", resultLocation);
        }

        [HttpPut]
        [Route("id/{id}")]
        public ActionResult Update(int id, [FromBody] PutLocationDto locationDto)
        {
            LocationDto resultLocation = locationService.Update(id, locationDto, User);

            return Ok(resultLocation);
        }

        [HttpDelete]
        [Route("id/{id}")]
        public ActionResult Delete(int id)
        {
            locationService.DeleteById(id, User);

            return NoContent();
        }
    }
}
