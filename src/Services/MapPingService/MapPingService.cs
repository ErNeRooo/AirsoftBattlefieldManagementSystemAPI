using System.Security.Claims;
using AirsoftBattlefieldManagementSystemAPI.Exceptions;
using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.MapPing;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Models.Helpers;
using AirsoftBattlefieldManagementSystemAPI.Realtime;
using AirsoftBattlefieldManagementSystemAPI.Services.AuthorizationHelperService;
using AirsoftBattlefieldManagementSystemAPI.Services.ClaimsHelperService;
using AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;

namespace AirsoftBattlefieldManagementSystemAPI.Services.MapPingService
{
    public class MapPingService(
        IMapper mapper, 
        IBattleManagementSystemDbContext dbContext, 
        IAuthorizationHelperService authorizationHelper, 
        IDbContextHelperService dbHelper, 
        IClaimsHelperService claimsHelper,
        IHubContext<RoomNotificationHub, IRoomNotificationClient> hubContext
        ) : IMapPingService
    {
        public MapPingDto GetById(int id, ClaimsPrincipal user)
        {
            MapPing mapPing = dbHelper.MapPing.FindById(id);
            Player player = dbHelper.Player.FindById(mapPing.PlayerId);
            Team team = dbHelper.Team.FindById(player.TeamId);

            authorizationHelper.CheckPlayerIsInTheSameTeamAsResource(user, team.TeamId, "You can't get map pings from different team.");
            
            MapPingDto mapPingDto = mapper.Map<MapPingDto>(mapPing);

            return mapPingDto;
        }

        public List<MapPingDto> GetManyByTeamId(int teamId, ClaimsPrincipal user)
        {
            bool doesTeamExist = dbContext.Team.Any(team => team.TeamId == teamId);
            if(!doesTeamExist) throw new NotFoundException("Team not found.");
            
            authorizationHelper.CheckPlayerIsInTheSameTeamAsResource(user, teamId, "You can't get map pings from different team.");
            
            List<MapPing> mapPings = dbHelper.MapPing.FindAllOfTeam(teamId);
            
            return mapPings.Select(mapPing => mapper.Map<MapPingDto>(mapPing)).ToList();
        }

        public MapPingDto Create(PostMapPingDto mapPingDto, ClaimsPrincipal user)
        {
            Player player = dbHelper.Player.FindById(mapPingDto.PlayerId);
            
            if (player.TeamId is null) throw new ForbidException("Can't create map ping when not in a team.");
            
            Room room = dbHelper.Room.FindByIdIncludingRelated(player.RoomId);

            if (room.Battle is null) throw new ForbidException("Can't create map ping because there is no battle.");
            
            Location location = mapper.Map<Location>(mapPingDto);
            dbContext.Location.Add(location);

            dbContext.SaveChanges();
            
            MapPing mapPing = new MapPing();
            mapPing.LocationId = location.LocationId;
            mapPing.PlayerId = player.PlayerId;
            mapPing.BattleId = room.Battle.BattleId;
            mapPing.Type = mapPingDto.Type;
            dbContext.MapPing.Add(mapPing);

            dbContext.SaveChanges();
            
            MapPingDto responseMapPingDto = mapper.Map<MapPingDto>(mapPing);            
            
            IEnumerable<string> playerIds = room.GetTeamPlayerIdsWithoutSelf(player.TeamId, player.PlayerId);

            hubContext.Clients.Users(playerIds).MapPingCreated(responseMapPingDto);

            return responseMapPingDto;
        }
        
        public void DeleteById(int id, ClaimsPrincipal user)
        {
            int clientPlayerId = claimsHelper.GetIntegerClaimValue("playerId", user);
            
            MapPing mapPing = dbHelper.MapPing.FindById(id);
            Player player = dbHelper.Player.FindById(mapPing.PlayerId);
            Team team = dbHelper.Team.FindById(player.TeamId);
            Room room = dbHelper.Room.FindByIdIncludingPlayers(player.RoomId);

            authorizationHelper.CheckTargetPlayerIsInTheSameTeam(user, mapPing.PlayerId, team.TeamId, "Cannot delete map ping because it is in a different team.");
            
            if (team.OfficerPlayerId != clientPlayerId)
            {
                authorizationHelper.CheckPlayerOwnsResource(user, mapPing.PlayerId);
            }
            
            dbContext.MapPing.Remove(mapPing);
            dbContext.SaveChanges();
            
            IEnumerable<string> playerIds = room.GetTeamPlayerIdsWithoutSelf(player.TeamId, player.PlayerId);

            hubContext.Clients.Users(playerIds).MapPingDeleted(id);
        }
    }
}
