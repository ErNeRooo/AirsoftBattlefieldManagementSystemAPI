using AirsoftBattlefieldManagementSystemAPI.Models.Entities;

namespace AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService.Helpers.PlayerLocationHelper;

public interface IPlayerLocationHelper
{
    public PlayerLocation FindById(int? id);
}