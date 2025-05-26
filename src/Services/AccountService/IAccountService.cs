using System.Security.Claims;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Account;

namespace AirsoftBattlefieldManagementSystemAPI.Services.AccountService
{
    public interface IAccountService
    {
        public AccountDto GetById(int id);
        public int Create(PostAccountDto accountDto, ClaimsPrincipal user);
        public int LogIn(LoginAccountDto accountDto, ClaimsPrincipal user);
        public void Update(int id, PutAccountDto accountDto);
        public void DeleteById(int id);
    }
}
