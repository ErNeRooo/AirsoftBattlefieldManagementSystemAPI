using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update;

namespace AirsoftBattlefieldManagementSystemAPI.Services.Abstractions
{
    public interface ITeamService
    {
        public TeamDto GetById(int id);
        public int Create(CreateTeamDto teamDto);
        public void Update(int id, UpdateTeamDto teamDto);
        public void DeleteById(int id);
    }
}
