using System.Security.Claims;
using AirsoftBattlefieldManagementSystemAPI.Exceptions;
using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Account;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Services.AuthorizationHelperService;
using AirsoftBattlefieldManagementSystemAPI.Services.ClaimsHelperService;
using AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace AirsoftBattlefieldManagementSystemAPI.Services.AccountService
{
    public class AccountService(IMapper mapper, IBattleManagementSystemDbContext dbContext, IPasswordHasher<Account> passwordHasher, IAuthorizationHelperService authorizationHelper, IDbContextHelperService dbHelper, IClaimsHelperService claimsHelper) : IAccountService
    {
        public AccountDto GetById(int id)
        {
            Account account = dbHelper.FindAccountById(id);

            AccountDto accountDto = mapper.Map<AccountDto>(account);

            return accountDto;
        }

        public AccountDto Create(PostAccountDto accountDto, ClaimsPrincipal user)
        {
            int playerId = claimsHelper.GetIntegerClaimValue("playerId", user);
            
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
            int playerId = claimsHelper.GetIntegerClaimValue("playerId", user);
            
            Account account = dbHelper.FindAccountByEmail(accountDto.Email);

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

        public AccountDto Update(PutAccountDto accountDto, ClaimsPrincipal user)
        {
            int playerId = claimsHelper.GetIntegerClaimValue("playerId", user);
            Player player = dbHelper.FindPlayerByIdIncludingAccount(playerId);
            Account previousAccount = dbHelper.FindAccountById(player.Account.AccountId);
            
            authorizationHelper.CheckPlayerOwnsResource(user, previousAccount.PlayerId);

            mapper.Map(accountDto, previousAccount);

            var hash = passwordHasher.HashPassword(previousAccount, previousAccount.PasswordHash);
            previousAccount.PasswordHash = hash;

            dbContext.Account.Update(previousAccount);
            dbContext.SaveChanges();
            
            return mapper.Map<AccountDto>(previousAccount);
        }

        public void Delete(ClaimsPrincipal user)
        {
            int playerId = claimsHelper.GetIntegerClaimValue("playerId", user);
            Player player = dbHelper.FindPlayerByIdIncludingAccount(playerId);
            Account account = dbHelper.FindAccountById(player.Account.AccountId);

            authorizationHelper.CheckPlayerOwnsResource(user, account.PlayerId);
            
            dbContext.Account.Remove(account);
            dbContext.SaveChanges();
        }
    }
}
