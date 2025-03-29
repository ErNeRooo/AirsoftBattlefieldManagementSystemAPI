using System.Security.Claims;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update;

namespace AirsoftBattlefieldManagementSystemAPI.Services.Abstractions
{
    public interface ITeamService
    {
        public TeamDto GetById(int id, ClaimsPrincipal user);
        public int Create(PostTeamDto postTeamDto, ClaimsPrincipal user);
        public void Update(int id, PutTeamDto teamDto, ClaimsPrincipal user);
        public void DeleteById(int id, ClaimsPrincipal user);
    }
}
