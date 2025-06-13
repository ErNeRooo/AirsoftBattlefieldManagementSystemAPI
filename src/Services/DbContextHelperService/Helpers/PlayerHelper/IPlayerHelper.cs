using System.Security.Claims;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;

namespace AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService.Helpers.PlayerHelper;

public interface IPlayerHelper
{
    public Player FindSelf(ClaimsPrincipal user);
    public Player FindById(int? id);
    public Player FindByIdIncludingAccount(int? id);
}