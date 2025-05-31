using System.Security.Claims;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Room;

namespace AirsoftBattlefieldManagementSystemAPI.Services.RoomService
{
    public interface IRoomService
    {
        public RoomDto GetById(int id);
        public RoomDto GetByJoinCode(string joinCode);
        public RoomDto Create(PostRoomDto roomDto, ClaimsPrincipal user);
        public RoomDto Update(int id, PutRoomDto roomDto, ClaimsPrincipal user);
        public void DeleteById(int id, ClaimsPrincipal user);
        public RoomDto Join(LoginRoomDto roomDto, ClaimsPrincipal user);
        public void Leave(ClaimsPrincipal user);
    }
}
