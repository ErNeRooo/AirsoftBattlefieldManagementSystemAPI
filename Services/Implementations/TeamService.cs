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

            if (team is null) return null;

            TeamDto teamDto = mapper.Map<TeamDto>(team);

            return teamDto;
        }

        public int Create(CreateTeamDto teamDto)
        {
            Team team = mapper.Map<Team>(teamDto);
            dbContext.Team.Add(team);
            dbContext.SaveChanges();

            return team.TeamId;
        }

        public bool Update(int id, UpdateTeamDto teamDto)
        {
            Team? previousTeam = dbContext.Team.FirstOrDefault(t => t.TeamId == id);

            if (previousTeam is null) return false;

            mapper.Map(teamDto, previousTeam);
            dbContext.Team.Update(previousTeam);
            dbContext.SaveChanges();

            return true;
        }

        public bool DeleteById(int id)
        {
            Team? team = dbContext.Team.FirstOrDefault(t => t.TeamId == id);

            if(team is null) return false;

            dbContext.Team.Remove(team);
            dbContext.SaveChanges();

            return true;
        }
    }
}
