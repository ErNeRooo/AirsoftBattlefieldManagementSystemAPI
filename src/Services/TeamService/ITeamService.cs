using System.Security.Claims;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Team;

namespace AirsoftBattlefieldManagementSystemAPI.Services.TeamService
{
    public interface ITeamService
    {
        public TeamDto GetById(int id, ClaimsPrincipal user);
        public int Create(PostTeamDto postTeamDto, ClaimsPrincipal user);
        public void Update(int id, PutTeamDto teamDto, ClaimsPrincipal user);
        public void DeleteById(int id, ClaimsPrincipal user);
    }
}
