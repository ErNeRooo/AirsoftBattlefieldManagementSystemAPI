using System.Security.Claims;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Death;

namespace AirsoftBattlefieldManagementSystemAPI.Services.DeathService
{
    public interface IDeathService
    {
        public DeathDto GetById(int id, ClaimsPrincipal user);
        public List<DeathDto> GetAllOfPlayerWithId(int playerId, ClaimsPrincipal user);
        public DeathDto Create(PostDeathDto postDeathDto, ClaimsPrincipal user);
        public DeathDto Update(int id, PutDeathDto deathDto, ClaimsPrincipal user);
        public void DeleteById(int id, ClaimsPrincipal user);
    }
}
