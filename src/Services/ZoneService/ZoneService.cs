using System.Security.Claims;
using AirsoftBattlefieldManagementSystemAPI.Authorization;
using AirsoftBattlefieldManagementSystemAPI.Exceptions;
using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Zone;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Models.Helpers;
using AirsoftBattlefieldManagementSystemAPI.Realtime;
using AirsoftBattlefieldManagementSystemAPI.Services.AuthorizationHelperService;
using AirsoftBattlefieldManagementSystemAPI.Services.ClaimsHelperService;
using AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace AirsoftBattlefieldManagementSystemAPI.Services.ZoneService;

public class ZoneService(
    IMapper mapper, 
    IBattleManagementSystemDbContext dbContext, 
    IDbContextHelperService dbHelper, 
    IAuthorizationHelperService authorizationHelperService,
    IClaimsHelperService claimsHelper,
    IHubContext<RoomNotificationHub, IRoomNotificationClient> hubContext
    ) : IZoneService
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
        int playerId = claimsHelper.GetIntegerClaimValue("playerId", user);
        Battle battle = dbHelper.Battle.FindById(zoneDto.BattleId);
        Room room = dbHelper.Room.FindByIdIncludingPlayers(battle.RoomId);
        
        authorizationHelperService.CheckPlayerIsInTheSameRoomAsResource(user, battle.RoomId);
        authorizationHelperService.CheckPlayerOwnsResource(user, room.AdminPlayerId);
            
        Zone zone = mapper.Map<Zone>(zoneDto);
            
        dbContext.Zone.Add(zone);
        dbContext.SaveChanges();
        
        ZoneDto responseZoneDto = mapper.Map<ZoneDto>(zone);
            
        IEnumerable<string> playerIds = room.GetAllPlayerIdsWithoutSelf(playerId);

        hubContext.Clients.Users(playerIds).ZoneCreated(responseZoneDto);

        return responseZoneDto;
    }

    public ZoneDto Update(int id, PutZoneDto zoneDto, ClaimsPrincipal user)
    {
        int playerId = claimsHelper.GetIntegerClaimValue("playerId", user);
        Zone zone = dbHelper.Zone.FindById(id);
        Battle battle = dbHelper.Battle.FindById(zone.BattleId);
        Room room = dbHelper.Room.FindByIdIncludingPlayers(battle.RoomId);
            
        authorizationHelperService.CheckPlayerIsInTheSameRoomAsResource(user, battle.RoomId);
        authorizationHelperService.CheckPlayerOwnsResource(user, room.AdminPlayerId);
        
        mapper.Map(zoneDto, zone);
            
        dbContext.Zone.Update(zone);
        dbContext.SaveChanges();
        
        ZoneDto responseZoneDto = mapper.Map<ZoneDto>(zone);
            
        IEnumerable<string> playerIds = room.GetAllPlayerIdsWithoutSelf(playerId);

        hubContext.Clients.Users(playerIds).ZoneUpdated(responseZoneDto);
            
        return responseZoneDto;
    }

    public void Delete(int id, ClaimsPrincipal user)
    {
        int playerId = claimsHelper.GetIntegerClaimValue("playerId", user);
        Zone targetZone = dbHelper.Zone.FindById(id);
        Battle battle = dbHelper.Battle.FindById(targetZone.BattleId);
        Room room = dbHelper.Room.FindByIdIncludingPlayers(battle.RoomId);
            
        authorizationHelperService.CheckPlayerIsInTheSameRoomAsResource(user, battle.RoomId);
        authorizationHelperService.CheckPlayerOwnsResource(user, room.AdminPlayerId);
        
        dbContext.Zone.Remove(targetZone);
        dbContext.SaveChanges();
        
        IEnumerable<string> playerIds = room.GetAllPlayerIdsWithoutSelf(playerId);

        hubContext.Clients.Users(playerIds).ZoneDeleted(id);
    }
}
