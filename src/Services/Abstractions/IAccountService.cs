using System.Security.Claims;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update;

namespace AirsoftBattlefieldManagementSystemAPI.Services.Abstractions
{
    public interface IAccountService
    {
        public AccountDto GetById(int id);
        public int Create(PostAccountDto accountDto, ClaimsPrincipal user);
        public void Update(int id, PutAccountDto accountDto);
        public void DeleteById(int id);
    }
}
