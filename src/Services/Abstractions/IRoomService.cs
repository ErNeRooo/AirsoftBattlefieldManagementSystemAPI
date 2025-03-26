using System.Security.Claims;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update;

namespace AirsoftBattlefieldManagementSystemAPI.Services.Abstractions
{
    public interface IRoomService
    {
        public RoomDto GetById(int id);
        public RoomDto GetByJoinCode(string joinCode);
        public string Create(PostRoomDto roomDto, ClaimsPrincipal user);
        public void Update(int id, PutRoomDto roomDto, ClaimsPrincipal user);
        public void DeleteById(int id, ClaimsPrincipal user);
    }
}
