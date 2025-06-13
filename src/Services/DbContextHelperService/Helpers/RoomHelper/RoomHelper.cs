using AirsoftBattlefieldManagementSystemAPI.Exceptions;
using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;

namespace AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService.Helpers.RoomHelper;

public class RoomHelper(IBattleManagementSystemDbContext dbContext) : IRoomHelper
{
    public Room FindById(int? id)
    {
        Room? room = dbContext.Room.FirstOrDefault(r => r.RoomId == id);

        if(room is null) throw new NotFoundException($"Room with id {id} not found");
            
        return room;
    }
        
    public Room FindByJoinCode(string joinCode)
    {
        Room? room = dbContext.Room.FirstOrDefault(r => r.JoinCode == joinCode);

        if(room is null) throw new NotFoundException($"Room with join code {joinCode} not found");
            
        return room;
    }
}