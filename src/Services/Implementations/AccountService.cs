using AirsoftBattlefieldManagementSystemAPI.Exceptions;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Services.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace AirsoftBattlefieldManagementSystemAPI.Services.Implementations
{
    public class AccountService(IMapper mapper, IBattleManagementSystemDbContext dbContext, IPasswordHasher<Account> passwordHasher) : IAccountService
    {
        public AccountDto? GetById(int id)
        {
            Account? account = dbContext.Account.FirstOrDefault(t => t.AccountId == id);

            if (account is null) throw new NotFoundException($"Account with id {id} not found");

            AccountDto accountDto = mapper.Map<AccountDto>(account);

            return accountDto;
        }

        public int Create(PostAccountDto accountDto)
        {
            Account account = mapper.Map<Account>(accountDto);

            if (account.PasswordHash is null)
            {
                account.PasswordHash = "";
            }
            else
            {
                var hash = passwordHasher.HashPassword(account, account.PasswordHash);
                account.PasswordHash = hash;
            }


            dbContext.Account.Add(account);
            dbContext.SaveChanges();

            return account.AccountId;
        }

        public void Update(int id, PutAccountDto accountDto)
        {
            Account? previousAccount = dbContext.Account.FirstOrDefault(t => t.AccountId == id);

            if (previousAccount is null) throw new NotFoundException($"Account with id {id} not found");

            mapper.Map(accountDto, previousAccount);

            if (previousAccount.PasswordHash is null)
            {
                previousAccount.PasswordHash = "";
            }
            else
            {
                var hash = passwordHasher.HashPassword(previousAccount, previousAccount.PasswordHash);
                previousAccount.PasswordHash = hash;
            }

            dbContext.Account.Update(previousAccount);
            dbContext.SaveChanges();
        }

        public void DeleteById(int id)
        {
            Account? account = dbContext.Account.FirstOrDefault(t => t.AccountId == id);

            if(account is null) throw new NotFoundException($"Account with id {id} not found");

            dbContext.Account.Remove(account);
            dbContext.SaveChanges();
        }
    }
}
