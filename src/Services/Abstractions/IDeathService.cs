using System.Security.Claims;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update;

namespace AirsoftBattlefieldManagementSystemAPI.Services.Abstractions
{
    public interface IDeathService
    {
        public DeathDto GetById(int id, ClaimsPrincipal user);
        public List<DeathDto> GetAllOfPlayerWithId(int playerId, ClaimsPrincipal user);
        public int Create(int playerId, PostDeathDto postDeathDto, ClaimsPrincipal user);
        public void Update(int id, PutDeathDto deathDto, ClaimsPrincipal user);
        public void DeleteById(int id, ClaimsPrincipal user);
    }
}
