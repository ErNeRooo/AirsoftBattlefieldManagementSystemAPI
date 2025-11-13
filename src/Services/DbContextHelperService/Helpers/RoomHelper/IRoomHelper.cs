using AirsoftBattlefieldManagementSystemAPI.Models.Entities;

namespace AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService.Helpers.RoomHelper;

public interface IRoomHelper
{
    public Room FindById(int? id);
    public Room FindByIdIncludingBattle(int? id);
    public Room FindByIdIncludingPlayers(int? id);
    public Room FindByIdIncludingRelated(int? id);
    public Room FindByJoinCodeIncludingRelated(string joinCode);
    public Room FindByIdIncludingAll(int? id);
    public Room FindByJoinCodeIncludingAll(string joinCode);
    public Room FindByJoinCode(string joinCode);
}