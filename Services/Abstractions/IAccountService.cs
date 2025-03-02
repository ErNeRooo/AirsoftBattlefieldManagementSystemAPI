using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update;

namespace AirsoftBattlefieldManagementSystemAPI.Services.Abstractions
{
    public interface IAccountService
    {
        public AccountDto? GetById(int id);
        public int Create(CreateAccountDto accountDto);
        public bool Update(int id, UpdateAccountDto accountDto);
        public bool DeleteById(int id);
    }
}
