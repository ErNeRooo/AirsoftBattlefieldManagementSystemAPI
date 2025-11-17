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

    public Room FindByIdIncludingAll(int? id)
    {
        var room = dbContext.Room
            .Include(r => r.Battle)
            .Include(r => r.AdminPlayer)
            .FirstOrDefault(r => r.RoomId == id);
        
        if(room is null) 
            throw new NotFoundException($"Room with id {id} not found");

        room.Teams = GetTeams(room.RoomId);
        room.Players = GetPlayers(room.RoomId);

        if (room.Battle is not null)
        {
            room.Battle.Orders = GetOrders(room.Battle.BattleId);
            room.Battle.Zones = GetZones(room.Battle.BattleId);
            room.Battle.MapPings = GetMapPings(room.Battle.BattleId);
            AssignDeathsToPlayers(room);
            AssignKillsToPlayers(room);
            AssignPlayerLocationsToPlayers(room);
        }

        return room;
    }
    
    public Room FindByJoinCodeIncludingAll(string joinCode)
    {
        var room = dbContext.Room
            .Include(r => r.Battle)
            .Include(r => r.AdminPlayer)
            .FirstOrDefault(r => r.JoinCode == joinCode);

        if (room is null)
            throw new NotFoundException($"Room with join code {joinCode} not found");

        room.Teams = GetTeams(room.RoomId);
        room.Players = GetPlayers(room.RoomId);

        if (room.Battle is not null)
        {
            room.Battle.Orders = GetOrders(room.Battle.BattleId);
            room.Battle.Zones = GetZones(room.Battle.BattleId);
            room.Battle.MapPings = GetMapPings(room.Battle.BattleId);
            AssignDeathsToPlayers(room);
            AssignKillsToPlayers(room);
            AssignPlayerLocationsToPlayers(room);
        }

        return room;
    }
    
    private List<Team> GetTeams(int roomId) =>
        dbContext.Team.Where(t => t.RoomId == roomId).ToList();

    private List<Player> GetPlayers(int roomId) =>
        dbContext.Player.Where(p => p.RoomId == roomId).ToList();

    private List<Order> GetOrders(int battleId) =>
        dbContext.Order.Include(o => o.Location).Where(o => o.BattleId == battleId).ToList();

    private List<Zone> GetZones(int battleId) =>
        dbContext.Zone.Include(z => z.Vertices).Where(z => z.BattleId == battleId).ToList();

    private List<MapPing> GetMapPings(int battleId) =>
        dbContext.MapPing.Include(mp => mp.Location).Where(mp => mp.BattleId == battleId).ToList();
    
    private void AssignDeathsToPlayers(Room room)
    {
        var deaths = dbContext.Death
            .Include(d => d.Location)
            .Where(d => d.BattleId == room.Battle!.BattleId)
            .ToList();

        foreach (var death in deaths)
        {
            var player = room.Players.FirstOrDefault(p => p.PlayerId == death.PlayerId);
            player?.Deaths.Add(death);
        }
    }

    private void AssignKillsToPlayers(Room room)
    {
        var kills = dbContext.Kill
            .Include(k => k.Location)
            .Where(k => k.BattleId == room.Battle!.BattleId)
            .ToList();

        foreach (var kill in kills)
        {
            var player = room.Players.FirstOrDefault(p => p.PlayerId == kill.PlayerId);
            player?.Kills.Add(kill);
        }
    }

    private void AssignPlayerLocationsToPlayers(Room room)
    {
        var locations = dbContext.PlayerLocation
            .Include(pl => pl.Location)
            .Where(pl => pl.BattleId == room.Battle!.BattleId)
            .ToList();

        foreach (var loc in locations)
        {
            var player = room.Players.FirstOrDefault(p => p.PlayerId == loc.PlayerId);
            player?.PlayerLocations.Add(loc);
        }
    }

    public Room FindByJoinCode(string joinCode)
    {
        Room? room = dbContext.Room.FirstOrDefault(r => r.JoinCode == joinCode);

        if(room is null) throw new NotFoundException($"Room with join code {joinCode} not found");
            
        return room;
    }
}