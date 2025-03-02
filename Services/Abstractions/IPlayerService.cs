using AirsoftBattlefieldManagementSystemAPI.Models.Dtos;

namespace AirsoftBattlefieldManagementSystemAPI.Services.Abstractions
{
    public interface IPlayerService
    {
        public PlayerDto? GetById(int id);
        public int Create(CreatePlayerDto playerDto);
        public bool Update(int id, UpdatePlayerDto playerDto);
        public bool DeleteById(int id);
    }
}
