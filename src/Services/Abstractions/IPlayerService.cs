using System.Security.Claims;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Login;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update;

namespace AirsoftBattlefieldManagementSystemAPI.Services.Abstractions
{
    public interface IPlayerService
    {
        public PlayerDto GetById(int id, ClaimsPrincipal user);
        public int Create(PostPlayerDto playerDto);
        public void Update(int id, PutPlayerDto playerDto, ClaimsPrincipal user);
        public void JoinRoom(int id, LoginRoomDto roomDto, ClaimsPrincipal user);
        public void LeaveRoom(ClaimsPrincipal user);
        public void DeleteById(int id, ClaimsPrincipal user);
        public string GenerateJwt(int id);
    }
}
