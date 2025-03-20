using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update;

namespace AirsoftBattlefieldManagementSystemAPI.Services.Abstractions
{
    public interface ILocationService
    {
        public LocationDto GetById(int id);
        public List<LocationDto> GetAllOfPlayerWithId(int playerId);
        public int Create(int playerId, PostLocationDto postLocationDto);
        public void Update(int id, PutLocationDto locationDto);
        public void DeleteById(int id);
    }
}
