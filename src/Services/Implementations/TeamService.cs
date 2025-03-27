using System.Security.Claims;
using AirsoftBattlefieldManagementSystemAPI.Authorization;
using AirsoftBattlefieldManagementSystemAPI.Exceptions;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Services.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace AirsoftBattlefieldManagementSystemAPI.Services.Implementations
{
    public class TeamService(IMapper mapper, IBattleManagementSystemDbContext dbContext, IAuthorizationService authorizationService) : ITeamService
    {
        public TeamDto? GetById(int id)
        {
            Team? team = dbContext.Team.FirstOrDefault(t => t.TeamId == id);

            if (team is null) throw new NotFoundException($"Team with id {id} not found");

            TeamDto teamDto = mapper.Map<TeamDto>(team);

            return teamDto;
        }

        public int Create(PostTeamDto teamDto, ClaimsPrincipal user)
        {
            var authorizationResult =
                authorizationService.AuthorizeAsync(user, teamDto.OfficerPlayerId,
                    new PlayerOwnsResourceRequirement()).Result;

            if (!authorizationResult.Succeeded) throw new ForbidException($"You're unauthorize to manipulate this resource");
            
            Team team = mapper.Map<Team>(teamDto);
            dbContext.Team.Add(team);
            dbContext.SaveChanges();

            return team.TeamId;
        }

        public void Update(int id, PutTeamDto teamDto, ClaimsPrincipal user)
        {
            Team? previousTeam = dbContext.Team.FirstOrDefault(t => t.TeamId == id);

            if (previousTeam is null) throw new NotFoundException($"Team with id {id} not found");
            
            var authorizationResult =
                authorizationService.AuthorizeAsync(user, previousTeam.OfficerPlayerId,
                    new PlayerOwnsResourceRequirement()).Result;

            if (!authorizationResult.Succeeded) throw new ForbidException($"You're unauthorize to manipulate this resource");

            mapper.Map(teamDto, previousTeam);
            dbContext.Team.Update(previousTeam);
            dbContext.SaveChanges();
        }

        public void DeleteById(int id, ClaimsPrincipal user)
        {
            Team? team = dbContext.Team.FirstOrDefault(t => t.TeamId == id);

            if(team is null) throw new NotFoundException($"Team with id {id} not found");

            var authorizationResult =
                authorizationService.AuthorizeAsync(user, team.OfficerPlayerId,
                    new PlayerOwnsResourceRequirement()).Result;

            if (!authorizationResult.Succeeded) throw new ForbidException($"You're unauthorize to manipulate this resource");
            
            dbContext.Team.Remove(team);
            dbContext.SaveChanges();
        }
    }
}
