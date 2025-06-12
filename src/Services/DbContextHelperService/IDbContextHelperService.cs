using AirsoftBattlefieldManagementSystemAPI.Models.Entities;

namespace AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService;

public interface IDbContextHelperService
{
    public Location FindLocationById(int? id);
    public List<Location> FindAllLocationsOfPlayer(Player player);
    public PlayerLocation FindPlayerLocationById(int? id);
    public Player FindPlayerById(int? id);
    public Player FindPlayerByIdIncludingAccount(int? id);
    public Team FindTeamById(int? id);
    public Kill FindKillById(int? id);
    public List<Kill> FindAllKillsOfPlayer(Player player);
    public Death FindDeathById(int? id);
    public List<Death> FindAllDeathsOfPlayer(Player player);
    public Account FindAccountById(int? id);
    public Account FindAccountByEmail(string email);
    public Battle FindBattleById(int? id);
    public Room FindRoomById(int? id);
    public Room FindRoomByIJoinCode(string joinCode);
}