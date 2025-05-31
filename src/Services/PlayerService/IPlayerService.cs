using System.Security.Claims;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Player;

namespace AirsoftBattlefieldManagementSystemAPI.Services.PlayerService
{
    public interface IPlayerService
    {
        public PlayerDto GetById(int id, ClaimsPrincipal user);
        public PlayerDto Create(PostPlayerDto playerDto);
        public PlayerDto Update(int id, PutPlayerDto playerDto, ClaimsPrincipal user);
        public void DeleteById(int id, ClaimsPrincipal user);
        public string GenerateJwt(int playerId);
    }
}
