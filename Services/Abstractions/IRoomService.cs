using AirsoftBattlefieldManagementSystemAPI.Models.Dtos;

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
