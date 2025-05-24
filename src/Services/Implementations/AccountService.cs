using System.Security.Claims;
using AirsoftBattlefieldManagementSystemAPI.Exceptions;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Login;
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

        public int Create(PostAccountDto accountDto, ClaimsPrincipal user)
        {
            int playerId = int.Parse(user.Claims.FirstOrDefault(c => c.Type == "playerId").Value);
            
            bool hasAccount = dbContext.Account.Any(a => a.PlayerId == playerId);
            
            if(hasAccount) throw new OnePlayerCannotHaveTwoAccountsException("You already have an account");
            
            Account account = mapper.Map<Account>(accountDto);
            
            var hash = passwordHasher.HashPassword(account, account.PasswordHash);
            account.PasswordHash = hash;
            account.PlayerId = playerId;
            
            dbContext.Account.Add(account);
            dbContext.SaveChanges();
            
            return account.AccountId;
        }

        public int LogIn(LoginAccountDto accountDto, ClaimsPrincipal user)
        {
            string stringPlayerId = user.Claims.FirstOrDefault(c => c.Type == "playerId").Value;

            bool isParsingSuccessfull = int.TryParse(stringPlayerId, out int playerId);

            if (!isParsingSuccessfull) throw new ForbidException($"Invalid claim playerId");
            
            Account? account = dbContext.Account.FirstOrDefault(a => a.Email == accountDto.Email);

            if (account is null) throw new NotFoundException($"Account with email {accountDto.Email} not found");

            var verificationResult = passwordHasher.VerifyHashedPassword(account, account.PasswordHash, accountDto.Password);

            if (verificationResult == PasswordVerificationResult.Success || verificationResult == PasswordVerificationResult.SuccessRehashNeeded)
            {
                account.PlayerId = playerId;
                dbContext.Account.Update(account);
                dbContext.SaveChanges();

                return account.AccountId;
            }
            
            throw new WrongPasswordException("Wrong account password");
        }

        public void Update(int id, PutAccountDto accountDto)
        {
            Account? previousAccount = dbContext.Account.FirstOrDefault(t => t.AccountId == id);

            if (previousAccount is null) throw new NotFoundException($"Account with id {id} not found");

            mapper.Map(accountDto, previousAccount);

            var hash = passwordHasher.HashPassword(previousAccount, previousAccount.PasswordHash);
            previousAccount.PasswordHash = hash;

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
