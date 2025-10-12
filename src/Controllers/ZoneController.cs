using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Zone;
using AirsoftBattlefieldManagementSystemAPI.Services.ZoneService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftBattlefieldManagementSystemAPI.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class ZoneController(IZoneService zoneService) : ControllerBase
{
    [HttpGet("id/{id}")]
    public ActionResult<ZoneDto> GetById(int id)
    {
        ZoneDto zoneDto = zoneService.GetById(id, User);
        
        return Ok(zoneDto);
    }
    
    [HttpGet("battleId/{battleId}")]
    public ActionResult<List<ZoneDto>> GetManyByBattleId(int battleId)
    {
        List<ZoneDto> zoneDtos = zoneService.GetManyByBattleId(battleId, User);
        
        return Ok(zoneDtos);
    }
    
    [HttpPost("")]
    public ActionResult<ZoneDto> Create([FromBody] PostZoneDto postZoneDto)
    {
        ZoneDto zoneDto = zoneService.Create(postZoneDto, User);
        
        return Created($"/zone/id/{zoneDto.ZoneId}", zoneDto);
    }
    
    [HttpPut("id/{id}")]
    public ActionResult<ZoneDto> Update([FromBody] PutZoneDto putZoneDto, int id)
    {
        ZoneDto zoneDto = zoneService.Update(id, putZoneDto, User);
        
        return Ok(zoneDto);
    }
    
    [HttpDelete("id/{id}")]
    public ActionResult<ZoneDto> Delete(int id)
    {
        zoneService.Delete(id, User);
        
        return NoContent();
    }
}