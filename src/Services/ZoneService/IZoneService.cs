using System.Security.Claims;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Zone;

namespace AirsoftBattlefieldManagementSystemAPI.Services.ZoneService
{
    public interface IZoneService
    {
        public ZoneDto GetById(int id, ClaimsPrincipal user);
        public List<ZoneDto> GetManyByBattleId(int battleId, ClaimsPrincipal user);
        public ZoneDto Create(PostZoneDto postZoneDto, ClaimsPrincipal user);
        public ZoneDto Update(int id, PutZoneDto zoneDto, ClaimsPrincipal user);
        public void Delete(int id, ClaimsPrincipal user);
    }
}
