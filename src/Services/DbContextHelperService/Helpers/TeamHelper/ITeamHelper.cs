using AirsoftBattlefieldManagementSystemAPI.Models.Entities;

namespace AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService.Helpers.TeamHelper;

public interface ITeamHelper
{
    public Team FindById(int? id);
    public Team FindByIdIncludingRoom(int? id);
}