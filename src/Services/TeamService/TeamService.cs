using System.Security.Claims;
using AirsoftBattlefieldManagementSystemAPI.Exceptions;
using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Team;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Zone;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Models.Helpers;
using AirsoftBattlefieldManagementSystemAPI.Realtime;
using AirsoftBattlefieldManagementSystemAPI.Services.AuthorizationHelperService;
using AirsoftBattlefieldManagementSystemAPI.Services.ClaimsHelperService;
using AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService;
using AirsoftBattlefieldManagementSystemAPI.Services.ZoneService;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;

namespace AirsoftBattlefieldManagementSystemAPI.Services.TeamService
{
    public class TeamService(
        IMapper mapper, 
        IBattleManagementSystemDbContext dbContext, 
        IDbContextHelperService dbHelper, 
        IAuthorizationHelperService authorizationHelperService, 
        IClaimsHelperService claimsHelper,
        IZoneService zoneService,
        IHubContext<RoomNotificationHub, IRoomNotificationClient> hubContext
        ) : ITeamService
    {
        public TeamDto GetById(int id, ClaimsPrincipal user)
        {
            Team? team = dbContext.Team.FirstOrDefault(t => t.TeamId == id);

            if (team is null) throw new NotFoundException($"Team with id {id} not found");
            
            authorizationHelperService.CheckPlayerIsInTheSameRoomAsResource(user, team.RoomId);
            
            TeamDto teamDto = mapper.Map<TeamDto>(team);
            return teamDto;
        }

        public TeamDto Create(PostTeamDto teamDto, ClaimsPrincipal user)
        {
            int playerId = claimsHelper.GetIntegerClaimValue("playerId", user);
            
            authorizationHelperService.CheckPlayerIsInTheSameRoomAsResource(user, teamDto.RoomId);
            
            Room room = dbHelper.Room.FindByIdIncludingPlayers(teamDto.RoomId);
            Team team = mapper.Map<Team>(teamDto);
            
            team.OfficerPlayerId = claimsHelper.GetIntegerClaimValue("playerId", user);
            
            dbContext.Team.Add(team);
            dbContext.SaveChanges();
            
            TeamDto responseTeamDto = mapper.Map<TeamDto>(team);            
            
            IEnumerable<string> playerIds = room.GetAllPlayerIdsWithoutSelf(playerId);

            hubContext.Clients.Users(playerIds).TeamCreated(responseTeamDto);

            return responseTeamDto;
        }

        public TeamDto Update(int id, PutTeamDto teamDto, ClaimsPrincipal user)
        {
            int playerId = claimsHelper.GetIntegerClaimValue("playerId", user);
            Team previousTeam = dbHelper.Team.FindByIdIncludingRoom(id);
            Room room = dbHelper.Room.FindByIdIncludingPlayers(previousTeam.RoomId);
            
            if(previousTeam.OfficerPlayerId is not null) authorizationHelperService.CheckPlayerIsRoomAdminOrTargetTeamOfficer(user, id);
            if(teamDto.OfficerPlayerId is not null) authorizationHelperService.CheckTargetPlayerIsInTheSameTeam(user, (int)teamDto.OfficerPlayerId, id);
            
            mapper.Map(teamDto, previousTeam);
            
            dbContext.Team.Update(previousTeam);
            dbContext.SaveChanges();
            
            TeamDto responseTeamDto = mapper.Map<TeamDto>(previousTeam);            
            
            IEnumerable<string> playerIds = room.GetAllPlayerIdsWithoutSelf(playerId);

            hubContext.Clients.Users(playerIds).TeamUpdated(responseTeamDto);
            
            return responseTeamDto;
        }

        public void Delete(int id, ClaimsPrincipal user)
        {
            int playerId = claimsHelper.GetIntegerClaimValue("playerId", user);
            Team team = dbHelper.Team.FindByIdIncludingRoom(id);
            Room room = dbHelper.Room.FindByIdIncludingPlayers(team.RoomId);

            authorizationHelperService.CheckPlayerIsRoomAdminOrTargetTeamOfficer(user, id);
            
            dbContext.Team.Remove(team);
            dbContext.SaveChanges();
            
            IEnumerable<string> playerIds = room.GetAllPlayerIdsWithoutSelf(playerId);

            hubContext.Clients.Users(playerIds).TeamDeleted(id);
        }

        public TeamDto CreateSpawn(int teamId, PostZoneDto postZoneDto, ClaimsPrincipal user)
        {
            int playerId = claimsHelper.GetIntegerClaimValue("playerId", user);
            Team team = dbHelper.Team.FindById(teamId);
            Room room = dbHelper.Room.FindByIdIncludingPlayers(team.RoomId);
            
            authorizationHelperService.CheckPlayerOwnsResource(user, team.Room.AdminPlayerId);

            ZoneDto zoneDto = zoneService.Create(postZoneDto, user);
            
            team.SpawnZoneId = zoneDto.ZoneId;
            
            dbContext.Team.Update(team);
            dbContext.SaveChanges();
            
            TeamDto responseTeamDto = mapper.Map<TeamDto>(team);            
            
            IEnumerable<string> playerIds = room.GetAllPlayerIdsWithoutSelf(playerId);

            hubContext.Clients.Users(playerIds).TeamUpdated(responseTeamDto);

            return responseTeamDto;
        }

        public void DeleteSpawn(int teamId, ClaimsPrincipal user)
        {
            int playerId = claimsHelper.GetIntegerClaimValue("playerId", user);
            Team team = dbHelper.Team.FindByIdIncludingRoom(teamId);
            Room room = dbHelper.Room.FindByIdIncludingPlayers(team.RoomId);
            
            authorizationHelperService.CheckPlayerOwnsResource(user, team.Room.AdminPlayerId);
            
            dbContext.Team.Remove(team);
            dbContext.SaveChanges();
            
            IEnumerable<string> playerIds = room.GetAllPlayerIdsWithoutSelf(playerId);

            hubContext.Clients.Users(playerIds).TeamDeleted(teamId);
        }

        public void Leave(ClaimsPrincipal user)
        {
            Player player = dbHelper.Player.FindSelf(user);
            Room room = dbHelper.Room.FindByIdIncludingPlayers(player.RoomId);

            player.Team = null;
            
            dbContext.Player.Update(player);
            dbContext.SaveChanges();
            
            IEnumerable<string> playerIds = room.GetAllPlayerIdsWithoutSelf(player.PlayerId);

            hubContext.Clients.Users(playerIds).PlayerLeftTeam(player.PlayerId);
        }
    }
}
