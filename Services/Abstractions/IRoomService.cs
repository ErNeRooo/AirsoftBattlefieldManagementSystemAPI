using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update;

namespace AirsoftBattlefieldManagementSystemAPI.Services.Abstractions
{
    public interface IRoomService
    {
        public RoomDto? GetById(int id);
        public int Create(CreateRoomDto roomDto);
        public bool Update(int id, UpdateRoomDto roomDto);
        public bool DeleteById(int id);
    }
}
