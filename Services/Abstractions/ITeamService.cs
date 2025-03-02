using AirsoftBattlefieldManagementSystemAPI.Models.Dtos;

namespace AirsoftBattlefieldManagementSystemAPI.Services.Abstractions
{
    public interface ITeamService
    {
        public TeamDto? GetById(int id);
        public int Create(CreateTeamDto teamDto);
        public bool Update(int id, UpdateTeamDto teamDto);
        public bool DeleteById(int id);
    }
}
