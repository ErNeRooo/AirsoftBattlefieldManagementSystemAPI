using AirsoftBattlefieldManagementSystemAPI.Exceptions;
using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService.Helpers.OrderHelper;

public class OrderHelper(IBattleManagementSystemDbContext dbContext) : IOrderHelper
{
    public Order FindById(int? id)
    {
        Order? order = dbContext.Order.Include(k => k.Location).FirstOrDefault(t => t.OrderId == id);

        if(order is null) throw new NotFoundException($"Order with id {id} not found");
            
        return order;
    }
    
    public Order FindByIdIncludingBattle(int? id)
    {
        Order? order = dbContext.Order
            .Include(k => k.Location)
            .Include(k => k.Battle)
            .FirstOrDefault(t => t.OrderId == id);

        if(order is null) throw new NotFoundException($"Order with id {id} not found");
            
        return order;
    }
    
    public List<Order> FindAllOfPlayer(Player player)
    {
        var orders = dbContext.Order
            .Include(k => k.Location)
            .Where(order => order.PlayerId == player.PlayerId).ToList();

        return orders;
    }
}