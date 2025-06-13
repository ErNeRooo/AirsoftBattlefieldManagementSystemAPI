using System.Security.Claims;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Player;

namespace AirsoftBattlefieldManagementSystemAPI.Services.PlayerService
{
    public interface IPlayerService
    {
        public PlayerDto GetMe(ClaimsPrincipal user);
        public PlayerDto GetById(int id, ClaimsPrincipal user);
        public PlayerDto Create(PostPlayerDto playerDto);
        public PlayerDto Update(PutPlayerDto playerDto, ClaimsPrincipal user);
        public void Delete(ClaimsPrincipal user);
        public string GenerateJwt(int playerId);
    }
}
