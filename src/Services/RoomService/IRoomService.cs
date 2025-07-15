using System.Security.Claims;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Room;

namespace AirsoftBattlefieldManagementSystemAPI.Services.RoomService
{
    public interface IRoomService
    {
        public RoomWithRelatedEntitiesDto GetById(int id);
        public RoomWithRelatedEntitiesDto GetByJoinCode(string joinCode);
        public RoomDto Create(PostRoomDto roomDto, ClaimsPrincipal user);
        public RoomDto Update(PutRoomDto roomDto, ClaimsPrincipal user);
        public void Delete(ClaimsPrincipal user);
        public RoomWithRelatedEntitiesDto Join(LoginRoomDto roomDto, ClaimsPrincipal user);
        public void Leave(ClaimsPrincipal user);
    }
}
