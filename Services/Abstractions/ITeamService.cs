using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update;

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
