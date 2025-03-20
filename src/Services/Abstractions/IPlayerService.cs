using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update;

namespace AirsoftBattlefieldManagementSystemAPI.Services.Abstractions
{
    public interface IPlayerService
    {
        public PlayerDto GetById(int id);
        public int Create(PostPlayerDto playerDto);
        public void Update(int id, PutPlayerDto playerDto);
        public void DeleteById(int id);
        public string GenerateJwt(int id);
    }
}
