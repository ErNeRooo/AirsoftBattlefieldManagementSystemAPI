using System.Security.Claims;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Battle;

namespace AirsoftBattlefieldManagementSystemAPI.Services.BattleService
{
    public interface IBattleService
    {
        public BattleDto GetById(int id, ClaimsPrincipal user);
        public int Create(PostBattleDto postBattleDto, ClaimsPrincipal user);
        public void Update(int id, PutBattleDto battleDto, ClaimsPrincipal user);
        public void DeleteById(int id, ClaimsPrincipal user);
    }
}
