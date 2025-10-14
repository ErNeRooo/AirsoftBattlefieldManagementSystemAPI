using System.Security.Claims;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Team;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Zone;

namespace AirsoftBattlefieldManagementSystemAPI.Services.TeamService
{
    public interface ITeamService
    {
        public TeamDto GetById(int id, ClaimsPrincipal user);
        public TeamDto Create(PostTeamDto postTeamDto, ClaimsPrincipal user);
        public TeamDto Update(int id, PutTeamDto teamDto, ClaimsPrincipal user);
        public void Leave(ClaimsPrincipal user);
        public void Delete(int id, ClaimsPrincipal user);
        public TeamDto CreateSpawn(int teamId, PostZoneDto postZoneDto, ClaimsPrincipal user);
        public void DeleteSpawn(int teamId, ClaimsPrincipal user);
    }
}
