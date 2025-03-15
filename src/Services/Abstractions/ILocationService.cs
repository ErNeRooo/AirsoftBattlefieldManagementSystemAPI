using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update;

namespace AirsoftBattlefieldManagementSystemAPI.Services.Abstractions
{
    public interface ILocationService
    {
        public LocationDto GetById(int id);
        public List<LocationDto> GetAllOfPlayerWithId(int playerId);
        public int Create(int playerId, CreateLocationDto locationDto);
        public void Update(int id, UpdateLocationDto locationDto);
        public void DeleteById(int id);
    }
}
