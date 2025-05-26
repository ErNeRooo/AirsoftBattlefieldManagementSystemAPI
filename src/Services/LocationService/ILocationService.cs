using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Location;
using System.Security.Claims;

namespace AirsoftBattlefieldManagementSystemAPI.Services.LocationService
{
    public interface ILocationService
    {
        public LocationDto GetById(int id, ClaimsPrincipal user);
        public List<LocationDto> GetAllOfPlayerWithId(int playerId, ClaimsPrincipal user);
        public int Create(int playerId, PostLocationDto postLocationDto, ClaimsPrincipal user);
        public void Update(int id, PutLocationDto locationDto, ClaimsPrincipal user);
        public void DeleteById(int id, ClaimsPrincipal user);
    }
}
