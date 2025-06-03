using System.Security.Claims;
using AirsoftBattlefieldManagementSystemAPI.Exceptions;
using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Account;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Services.AuthorizationHelperService;
using AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace AirsoftBattlefieldManagementSystemAPI.Services.AccountService
{
    public class AccountService(IMapper mapper, IBattleManagementSystemDbContext dbContext, IPasswordHasher<Account> passwordHasher, IAuthorizationHelperService authorizationHelper, IDbContextHelperService dbHelper) : IAccountService
    {
        public AccountDto GetById(int id)
        {
            Account account = dbHelper.FindAccountById(id);

            AccountDto accountDto = mapper.Map<AccountDto>(account);

            return accountDto;
        }

        public AccountDto Create(PostAccountDto accountDto, ClaimsPrincipal user)
        {
            int playerId = GetPlayerIdFromClaims(user);
            
            bool hasAccount = dbContext.Account.Any(a => a.PlayerId == playerId);
            
            if(hasAccount) throw new OnePlayerCannotHaveTwoAccountsException("You already have an account");
            
            Account account = mapper.Map<Account>(accountDto);
            
            var hash = passwordHasher.HashPassword(account, account.PasswordHash);
            account.PasswordHash = hash;
            account.PlayerId = playerId;
            
            dbContext.Account.Add(account);
            dbContext.SaveChanges();
            
            return mapper.Map<AccountDto>(account);
        }

        public AccountDto LogIn(LoginAccountDto accountDto, ClaimsPrincipal user)
        {
            int playerId = GetPlayerIdFromClaims(user);
            
            Account account = dbHelper.FindAccountById(playerId);

            var verificationResult = passwordHasher.VerifyHashedPassword(account, account.PasswordHash, accountDto.Password);

            if (verificationResult == PasswordVerificationResult.Success || verificationResult == PasswordVerificationResult.SuccessRehashNeeded)
            {
                account.PlayerId = playerId;
                dbContext.Account.Update(account);
                dbContext.SaveChanges();

                return mapper.Map<AccountDto>(account);
            }
            
            throw new WrongPasswordException("Wrong account password");
        }

        public AccountDto Update(int id, PutAccountDto accountDto)
        {
            Account previousAccount = dbHelper.FindAccountById(id);

            mapper.Map(accountDto, previousAccount);

            var hash = passwordHasher.HashPassword(previousAccount, previousAccount.PasswordHash);
            previousAccount.PasswordHash = hash;

            dbContext.Account.Update(previousAccount);
            dbContext.SaveChanges();
            
            return mapper.Map<AccountDto>(previousAccount);
        }

        public void DeleteById(int id)
        {
            Account account = dbHelper.FindAccountById(id);

            dbContext.Account.Remove(account);
            dbContext.SaveChanges();
        }
        
        private int GetPlayerIdFromClaims(ClaimsPrincipal user)
        {
            var playerIdClaim = user.Claims.FirstOrDefault(c => c.Type == "playerId")?.Value;
            
            bool isParsingSuccessfull = int.TryParse(playerIdClaim, out int playerId);
            
            if (!isParsingSuccessfull) throw new ForbidException("Invalid claim playerId");
            
            return playerId;
        }
    }
}
