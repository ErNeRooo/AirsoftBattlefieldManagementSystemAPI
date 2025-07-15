using AirsoftBattlefieldManagementSystemAPI.Models.Entities;

namespace AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService.Helpers.RoomHelper;

public interface IRoomHelper
{
    public Room FindById(int? id);
    public Room FindByIdIncludingBattle(int? id);
    public Room FindByIdIncludingRelated(int? id);
    public Room FindByJoinCodeIncludingRelated(string joinCode);
    public Room FindByJoinCode(string joinCode);
}