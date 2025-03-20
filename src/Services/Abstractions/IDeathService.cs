using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update;

namespace AirsoftBattlefieldManagementSystemAPI.Services.Abstractions
{
    public interface IDeathService
    {
        public DeathDto GetById(int id);
        public List<DeathDto> GetAllOfPlayerWithId(int playerId);
        public int Create(int playerId, PostDeathDto postDeathDto);
        public void Update(int id, PutDeathDto deathDto);
        public void DeleteById(int id);
    }
}
