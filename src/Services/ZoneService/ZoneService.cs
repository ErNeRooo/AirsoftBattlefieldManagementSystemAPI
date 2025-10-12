using System.Security.Claims;
using AirsoftBattlefieldManagementSystemAPI.Authorization;
using AirsoftBattlefieldManagementSystemAPI.Exceptions;
using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Zone;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Services.AuthorizationHelperService;
using AirsoftBattlefieldManagementSystemAPI.Services.ClaimsHelperService;
using AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace AirsoftBattlefieldManagementSystemAPI.Services.ZoneService;

public class ZoneService(IMapper mapper, IBattleManagementSystemDbContext dbContext, IDbContextHelperService dbHelper, IAuthorizationHelperService authorizationHelperService, IClaimsHelperService claimsHelper) : IZoneService
{
    public ZoneDto GetById(int id, ClaimsPrincipal user)
    {
        Zone zone = dbHelper.Zone.FindById(id);
        Battle battle = dbHelper.Battle.FindById(zone.BattleId);
        
        authorizationHelperService.CheckPlayerIsInTheSameRoomAsResource(user, battle.RoomId);
        
        ZoneDto zoneDto = mapper.Map<ZoneDto>(zone);
        return zoneDto;
    }

    public List<ZoneDto> GetManyByBattleId(int battleId, ClaimsPrincipal user)
    {
        List<Zone> zones = dbHelper.Zone.FindAllOfBattle(battleId);
        Battle battle = dbHelper.Battle.FindById(battleId);
        
        authorizationHelperService.CheckPlayerIsInTheSameRoomAsResource(user, battle.RoomId);
        
        List<ZoneDto> zoneDto = mapper.Map<List<ZoneDto>>(zones);
        return zoneDto;
    }

    public ZoneDto Create(PostZoneDto zoneDto, ClaimsPrincipal user)
    {
        Battle battle = dbHelper.Battle.FindById(zoneDto.BattleId);
        Room room = dbHelper.Room.FindById(battle.RoomId);
        
        authorizationHelperService.CheckPlayerIsInTheSameRoomAsResource(user, battle.RoomId);
        authorizationHelperService.CheckPlayerOwnsResource(user, room.AdminPlayerId);
            
        Zone zone = mapper.Map<Zone>(zoneDto);
            
        dbContext.Zone.Add(zone);
        dbContext.SaveChanges();

        return mapper.Map<ZoneDto>(zone);
    }

    public ZoneDto Update(int id, PutZoneDto zoneDto, ClaimsPrincipal user)
    {
        Zone previousZone = dbHelper.Zone.FindById(id);
        Battle battle = dbHelper.Battle.FindById(previousZone.BattleId);
        Room room = dbHelper.Room.FindById(battle.RoomId);
            
        authorizationHelperService.CheckPlayerIsInTheSameRoomAsResource(user, battle.RoomId);
        authorizationHelperService.CheckPlayerOwnsResource(user, room.AdminPlayerId);
        
        mapper.Map(zoneDto, previousZone);
            
        dbContext.Zone.Update(previousZone);
        dbContext.SaveChanges();
            
        return mapper.Map<ZoneDto>(previousZone);
    }

    public void Delete(int id, ClaimsPrincipal user)
    {
        Zone targetZone = dbHelper.Zone.FindById(id);
        Battle battle = dbHelper.Battle.FindById(targetZone.BattleId);
        Room room = dbHelper.Room.FindById(battle.RoomId);
            
        authorizationHelperService.CheckPlayerIsInTheSameRoomAsResource(user, battle.RoomId);
        authorizationHelperService.CheckPlayerOwnsResource(user, room.AdminPlayerId);
        
        dbContext.Zone.Remove(targetZone);
        dbContext.SaveChanges();
    }
}
