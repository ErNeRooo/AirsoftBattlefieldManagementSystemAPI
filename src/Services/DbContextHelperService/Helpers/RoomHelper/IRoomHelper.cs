using AirsoftBattlefieldManagementSystemAPI.Models.Entities;

namespace AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService.Helpers.RoomHelper;

public interface IRoomHelper
{
    public Room FindById(int? id);
    public Room FindByJoinCode(string joinCode);
}