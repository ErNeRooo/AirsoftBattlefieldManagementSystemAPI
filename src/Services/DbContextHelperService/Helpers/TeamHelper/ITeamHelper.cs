using AirsoftBattlefieldManagementSystemAPI.Models.Entities;

namespace AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService.Helpers.TeamHelper;

public interface ITeamHelper
{
    public Team FindById(int? id);
}