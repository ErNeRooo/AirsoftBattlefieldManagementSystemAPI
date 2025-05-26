using System.Security.Claims;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Player;

namespace AirsoftBattlefieldManagementSystemAPI.Services.PlayerService
{
    public interface IPlayerService
    {
        public PlayerDto GetById(int id, ClaimsPrincipal user);
        public int Create(PostPlayerDto playerDto);
        public void Update(int id, PutPlayerDto playerDto, ClaimsPrincipal user);
        public void DeleteById(int id, ClaimsPrincipal user);
        public string GenerateJwt(int playerId);
    }
}
