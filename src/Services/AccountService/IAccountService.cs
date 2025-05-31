using System.Security.Claims;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Account;

namespace AirsoftBattlefieldManagementSystemAPI.Services.AccountService
{
    public interface IAccountService
    {
        public AccountDto GetById(int id);
        public AccountDto Create(PostAccountDto accountDto, ClaimsPrincipal user);
        public AccountDto LogIn(LoginAccountDto accountDto, ClaimsPrincipal user);
        public AccountDto Update(int id, PutAccountDto accountDto);
        public void DeleteById(int id);
    }
}
