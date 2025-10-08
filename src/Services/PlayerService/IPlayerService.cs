using System.Security.Claims;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Player;

namespace AirsoftBattlefieldManagementSystemAPI.Services.PlayerService
{
    public interface IPlayerService
    {
        public PlayerDto GetMe(ClaimsPrincipal user);
        public PlayerDto GetById(int playerId, ClaimsPrincipal user);
        public PlayerDto Create(PostPlayerDto playerDto);
        public PlayerDto Update(PutPlayerDto playerDto, ClaimsPrincipal user);
        public PlayerDto KickFromRoom(int playerId, ClaimsPrincipal user);
        public PlayerDto KickFromTeam(int playerId, ClaimsPrincipal user);
        public void Delete(ClaimsPrincipal user);
        public string GenerateJwt(int playerId);
    }
}
