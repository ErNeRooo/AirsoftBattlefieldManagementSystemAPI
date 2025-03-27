using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update;
using System.Security.Claims;

namespace AirsoftBattlefieldManagementSystemAPI.Services.Abstractions
{
    public interface ILocationService
    {
        public LocationDto GetById(int id);
        public List<LocationDto> GetAllOfPlayerWithId(int playerId);
        public int Create(int playerId, PostLocationDto postLocationDto, ClaimsPrincipal user);
        public void Update(int id, PutLocationDto locationDto, ClaimsPrincipal user);
        public void DeleteById(int id, ClaimsPrincipal user);
    }
}
