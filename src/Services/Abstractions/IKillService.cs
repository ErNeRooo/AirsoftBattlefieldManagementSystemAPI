using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update;

namespace AirsoftBattlefieldManagementSystemAPI.Services.Abstractions
{
    public interface IKillService
    {
        public KillDto? GetById(int id);
        public List<KillDto>? GetAllOfPlayerWithId(int playerId);
        public int? Create(int playerId, CreateKillDto killDto);
        public bool Update(int id, UpdateKillDto killDto);
        public bool DeleteById(int id);
    }
}
