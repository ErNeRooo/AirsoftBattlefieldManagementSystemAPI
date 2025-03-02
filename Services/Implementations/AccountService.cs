using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Services.Abstractions;
using AutoMapper;

namespace AirsoftBattlefieldManagementSystemAPI.Services.Implementations
{
    public class AccountService(IMapper mapper, IBattleManagementSystemDbContext dbContext) : IAccountService
    {
        public AccountDto? GetById(int id)
        {
            Account? account = dbContext.Account.FirstOrDefault(t => t.AccountId == id);

            if (account is null) return null;

            AccountDto accountDto = mapper.Map<AccountDto>(account);

            return accountDto;
        }

        public int Create(CreateAccountDto accountDto)
        {
            Account account = mapper.Map<Account>(accountDto);
            dbContext.Account.Add(account);
            dbContext.SaveChanges();

            return account.AccountId;
        }

        public bool Update(int id, UpdateAccountDto accountDto)
        {
            Account? previousAccount = dbContext.Account.FirstOrDefault(t => t.AccountId == id);

            if (previousAccount is null) return false;

            mapper.Map(accountDto, previousAccount);
            dbContext.Account.Update(previousAccount);
            dbContext.SaveChanges();

            return true;
        }

        public bool DeleteById(int id)
        {
            Account? account = dbContext.Account.FirstOrDefault(t => t.AccountId == id);

            if(account is null) return false;

            dbContext.Account.Remove(account);
            dbContext.SaveChanges();

            return true;
        }
    }
}
