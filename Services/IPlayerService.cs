using AirsoftBattlefieldManagementSystemAPI.Models.Dtos;

namespace AirsoftBattlefieldManagementSystemAPI.Services
{
    public interface IPlayerService
    {
        public PlayerDto? GetById(int id);
        public int Create(CreatePlayerDto playerDto);
    }
}
