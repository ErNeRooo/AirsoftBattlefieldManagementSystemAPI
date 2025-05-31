using System.Security.Claims;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Kill;

namespace AirsoftBattlefieldManagementSystemAPI.Services.KillService
{
    public interface IKillService
    {
        public KillDto GetById(int id, ClaimsPrincipal user);
        public List<KillDto> GetAllOfPlayerWithId(int playerId, ClaimsPrincipal user);
        public KillDto Create(int playerId, PostKillDto postKillDto, ClaimsPrincipal user);
        public KillDto Update(int id, PutKillDto killDto, ClaimsPrincipal user);
        public void DeleteById(int id, ClaimsPrincipal user);
    }
}
