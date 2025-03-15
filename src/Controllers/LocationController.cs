﻿using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftBattlefieldManagementSystemAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LocationController(ILocationService locationService) : Controller
    {
        [HttpGet]
        [Route("id/{id}")]
        public ActionResult<LocationDto> GetById(int id)
        {
            LocationDto locationDto = locationService.GetById(id);

            return Ok(locationDto);
        }

        [HttpGet]
        [Route("playerId/{playerId}")]
        public ActionResult<List<LocationDto>> GetLocationsOfPlayerWithId(int playerId)
        {
            List<LocationDto> locationDtos = locationService.GetAllOfPlayerWithId(playerId);

            return Ok(locationDtos);
        }

        [HttpPost]
        [Route("playerId/{playerId}")]
        public ActionResult Create(int playerId, [FromBody] CreateLocationDto locationDto)
        {
            int id = locationService.Create(playerId, locationDto);

            return Created($"/Location/id/{id}", null);
        }

        [HttpPut]
        [Route("id/{id}")]
        public ActionResult Update(int id, [FromBody] UpdateLocationDto locationDto)
        {
            locationService.Update(id, locationDto);

            return Ok();
        }

        [HttpDelete]
        [Route("id/{id}")]
        public ActionResult Delete(int id)
        {
            locationService.DeleteById(id);

            return NotFound();
        }
    }
}
