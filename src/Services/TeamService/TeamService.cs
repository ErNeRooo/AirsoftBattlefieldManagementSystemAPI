using System.Security.Claims;
using AirsoftBattlefieldManagementSystemAPI.Authorization;
using AirsoftBattlefieldManagementSystemAPI.Exceptions;
using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Team;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Services.AuthorizationHelperService;
using AirsoftBattlefieldManagementSystemAPI.Services.ClaimsHelperService;
using AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace AirsoftBattlefieldManagementSystemAPI.Services.TeamService
{
    public class TeamService(IMapper mapper, IBattleManagementSystemDbContext dbContext, IDbContextHelperService dbHelper, IAuthorizationHelperService authorizationHelperService, IClaimsHelperService claimsHelper) : ITeamService
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
            
            team.OfficerPlayerId = claimsHelper.GetIntegerClaimValue("playerId", user);
            
            dbContext.Team.Add(team);
            dbContext.SaveChanges();

            return mapper.Map<TeamDto>(team);
        }

        public TeamDto Update(int id, PutTeamDto teamDto, ClaimsPrincipal user)
        {
            Team previousTeam = dbHelper.Team.FindById(id);
            
            if(previousTeam.OfficerPlayerId is not null) authorizationHelperService.CheckPlayerIsRoomAdminOrTargetTeamOfficer(user, id);
            if(teamDto.OfficerPlayerId is not null) authorizationHelperService.CheckTargetPlayerIsInTheSameTeam(user, (int)teamDto.OfficerPlayerId, id);
            
            mapper.Map(teamDto, previousTeam);
            
            dbContext.Team.Update(previousTeam);
            dbContext.SaveChanges();
            
            return mapper.Map<TeamDto>(previousTeam);
        }

        public void Delete(int id, ClaimsPrincipal user)
        {
            Team team = dbHelper.Team.FindById(id);

            authorizationHelperService.CheckPlayerIsRoomAdminOrTargetTeamOfficer(user, id);
            
            dbContext.Team.Remove(team);
            dbContext.SaveChanges();
        }
        
        public void Leave(ClaimsPrincipal user)
        {
            Player player = dbHelper.Player.FindSelf(user);

            player.Team = null;
            
            dbContext.Player.Update(player);
            dbContext.SaveChanges();
        }
    }
}
