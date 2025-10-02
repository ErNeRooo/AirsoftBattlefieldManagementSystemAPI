using AirsoftBattlefieldManagementSystemAPI.Models.Entities;

namespace AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService.Helpers.OrderHelper;

public interface IOrderHelper
{
    public Order FindById(int? id);
    public Order FindByIdIncludingBattle(int? id);
    public List<Order> FindAllOfPlayer(Player player);
}