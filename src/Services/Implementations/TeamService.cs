using AirsoftBattlefieldManagementSystemAPI.Exceptions;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Services.Abstractions;
using AutoMapper;

namespace AirsoftBattlefieldManagementSystemAPI.Services.Implementations
{
    public class TeamService(IMapper mapper, IBattleManagementSystemDbContext dbContext) : ITeamService
    {
        public TeamDto? GetById(int id)
        {
            Team? team = dbContext.Team.FirstOrDefault(t => t.TeamId == id);

            if (team is null) throw new NotFoundException($"Team with id {id} not found");

            TeamDto teamDto = mapper.Map<TeamDto>(team);

            return teamDto;
        }

        public int Create(PostTeamDto teamDto)
        {
            Team team = mapper.Map<Team>(teamDto);
            dbContext.Team.Add(team);
            dbContext.SaveChanges();

            return team.TeamId;
        }

        public void Update(int id, PutTeamDto teamDto)
        {
            Team? previousTeam = dbContext.Team.FirstOrDefault(t => t.TeamId == id);

            if (previousTeam is null) throw new NotFoundException($"Team with id {id} not found");

            mapper.Map(teamDto, previousTeam);
            dbContext.Team.Update(previousTeam);
            dbContext.SaveChanges();
        }

        public void DeleteById(int id)
        {
            Team? team = dbContext.Team.FirstOrDefault(t => t.TeamId == id);

            if(team is null) throw new NotFoundException($"Team with id {id} not found");

            dbContext.Team.Remove(team);
            dbContext.SaveChanges();
        }
    }
}
