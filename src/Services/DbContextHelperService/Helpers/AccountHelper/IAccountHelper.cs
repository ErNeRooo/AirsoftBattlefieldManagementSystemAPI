using AirsoftBattlefieldManagementSystemAPI.Models.Entities;

namespace AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService.Helpers.AccountHelper;

public interface IAccountHelper
{
    public Account FindById(int? id);
    public Account FindByEmail(string email);
}