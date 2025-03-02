using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update;

namespace AirsoftBattlefieldManagementSystemAPI.Services.Abstractions
{
    public interface IBattleService
    {
        public BattleDto? GetById(int id);
        public int Create(CreateBattleDto battleDto);
        public bool Update(int id, UpdateBattleDto battleDto);
        public bool DeleteById(int id);
    }
}
