using System.Security.Claims;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.MapPing;

namespace AirsoftBattlefieldManagementSystemAPI.Services.MapPingService
{
    public interface IMapPingService
    {
        public MapPingDto GetById(int id, ClaimsPrincipal user);
        public List<MapPingDto> GetManyByTeamId(int teamId, ClaimsPrincipal user);
        public MapPingDto Create(PostMapPingDto postMapPingDto, ClaimsPrincipal user);
        public void DeleteById(int id, ClaimsPrincipal user);
    }
}
