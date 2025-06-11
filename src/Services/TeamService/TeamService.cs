using System.Security.Claims;
using AirsoftBattlefieldManagementSystemAPI.Authorization;
using AirsoftBattlefieldManagementSystemAPI.Exceptions;
using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Team;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Services.AuthorizationHelperService;
using AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace AirsoftBattlefieldManagementSystemAPI.Services.TeamService
{
    public class TeamService(IMapper mapper, IBattleManagementSystemDbContext dbContext, IDbContextHelperService dbHelper, IAuthorizationHelperService authorizationHelperService) : ITeamService
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
            authorizationHelperService.CheckPlayerIsInTheSameRoomAsResource(user, teamDto.RoomId);
            
            Team team = mapper.Map<Team>(teamDto);
            
            team.OfficerPlayerId = GetPlayerIdFromClaims(user);
            
            dbContext.Team.Add(team);
            dbContext.SaveChanges();

            return mapper.Map<TeamDto>(team);
        }

        public TeamDto Update(int id, PutTeamDto teamDto, ClaimsPrincipal user)
        {
            Team previousTeam = dbHelper.FindTeamById(id);

            authorizationHelperService.CheckPlayerOwnsResource(user, previousTeam.OfficerPlayerId);
            
            mapper.Map(teamDto, previousTeam);
            
            dbContext.Team.Update(previousTeam);
            dbContext.SaveChanges();
            
            return mapper.Map<TeamDto>(previousTeam);
        }

        public void Delete(int id, ClaimsPrincipal user)
        {
            Team team = dbHelper.FindTeamById(id);

            authorizationHelperService.CheckPlayerOwnsResource(user, team.OfficerPlayerId);
            
            dbContext.Team.Remove(team);
            dbContext.SaveChanges();
        }

        private int GetPlayerIdFromClaims(ClaimsPrincipal user)
        {
            string? playerIdClaim = user.Claims.FirstOrDefault(c => c.Type == "playerId").Value;
            
            bool isParsingSuccessfull = int.TryParse(playerIdClaim, out int playerId);
            
            if(!isParsingSuccessfull) throw new Exception("Can't get player id from claim");
            
            return playerId;
        }
    }
}
