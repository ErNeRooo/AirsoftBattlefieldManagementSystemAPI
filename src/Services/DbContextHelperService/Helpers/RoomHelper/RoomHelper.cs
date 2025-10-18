using AirsoftBattlefieldManagementSystemAPI.Exceptions;
using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService.Helpers.RoomHelper;

public class RoomHelper(IBattleManagementSystemDbContext dbContext) : IRoomHelper
{
    public Room FindById(int? id)
    {
        Room? room = dbContext.Room.FirstOrDefault(r => r.RoomId == id);

        if(room is null) throw new NotFoundException($"Room with id {id} not found");
            
        return room;
    }
    
    public Room FindByIdIncludingBattle(int? id)
    {
        Room? room = dbContext.Room
            .Include(room => room.Battle)
            .FirstOrDefault(r => r.RoomId == id);

        if(room is null) throw new NotFoundException($"Room with id {id} not found");
            
        return room;
    }
    
    public Room FindByIdIncludingPlayers(int? id)
    {
        Room? room = dbContext.Room
            .Include(room => room.Players)
            .FirstOrDefault(r => r.RoomId == id);

        if(room is null) throw new NotFoundException($"Room with id {id} not found");
            
        return room;
    }
        
    public Room FindByIdIncludingRelated(int? id)
    {
        Room? room = dbContext.Room
            .Include(room => room.Battle)
            .Include(room => room.Teams)
            .Include(room => room.AdminPlayer)
            .Include(room => room.Players)
            .FirstOrDefault(r => r.RoomId == id);

        if(room is null) throw new NotFoundException($"Room with id {id} not found");
            
        return room;
    }
    
    public Room FindByJoinCodeIncludingRelated(string joinCode)
    {
        Room? room = dbContext.Room
            .Include(room => room.Battle)
            .Include(room => room.Teams)
            .Include(room => room.AdminPlayer)
            .Include(room => room.Players)
            .FirstOrDefault(r => r.JoinCode == joinCode);

        if(room is null) throw new NotFoundException($"Room with join code {joinCode} not found");
            
        return room;
    }
    
    public Room FindByJoinCode(string joinCode)
    {
        Room? room = dbContext.Room.FirstOrDefault(r => r.JoinCode == joinCode);

        if(room is null) throw new NotFoundException($"Room with join code {joinCode} not found");
            
        return room;
    }
}