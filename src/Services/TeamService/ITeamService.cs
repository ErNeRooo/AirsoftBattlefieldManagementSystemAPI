using System.Security.Claims;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Team;

namespace AirsoftBattlefieldManagementSystemAPI.Services.TeamService
{
    public interface ITeamService
    {
        public TeamDto GetById(int id, ClaimsPrincipal user);
        public TeamDto Create(PostTeamDto postTeamDto, ClaimsPrincipal user);
        public TeamDto Update(int id, PutTeamDto teamDto, ClaimsPrincipal user);
        public void Delete(int id, ClaimsPrincipal user);
    }
}
