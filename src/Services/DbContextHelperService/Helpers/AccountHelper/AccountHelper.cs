using AirsoftBattlefieldManagementSystemAPI.Exceptions;
using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;

namespace AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService.Helpers.AccountHelper;

public class AccountHelper(IBattleManagementSystemDbContext dbContext) : IAccountHelper
{
    public Account FindById(int? id)
    {
        Account? account = dbContext.Account.FirstOrDefault(t => t.AccountId == id);

        if(account is null) throw new NotFoundException($"Account with id {id} not found");
            
        return account;
    }
    
    public Account FindByEmail(string email)
    {
        Account? account = dbContext.Account.FirstOrDefault(t => t.Email == email);

        if(account is null) throw new NotFoundException($"Account with email {email} not found");
            
        return account;
    }
}