using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update;

namespace AirsoftBattlefieldManagementSystemAPI.Services.Abstractions
{
    public interface IKillService
    {
        public KillDto GetById(int id);
        public List<KillDto> GetAllOfPlayerWithId(int playerId);
        public int Create(int playerId, PostKillDto postKillDto);
        public void Update(int id, PutKillDto killDto);
        public void DeleteById(int id);
    }
}
