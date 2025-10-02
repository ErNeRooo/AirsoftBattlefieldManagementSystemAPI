using System.Security.Claims;
using AirsoftBattlefieldManagementSystemAPI.Exceptions;
using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Order;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Services.AuthorizationHelperService;
using AirsoftBattlefieldManagementSystemAPI.Services.ClaimsHelperService;
using AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService;
using AutoMapper;

namespace AirsoftBattlefieldManagementSystemAPI.Services.OrderService
{
    public class OrderService(IMapper mapper, IBattleManagementSystemDbContext dbContext, IAuthorizationHelperService authorizationHelper, IDbContextHelperService dbHelper, IClaimsHelperService claimsHelper) : IOrderService
    {
        public OrderDto GetById(int id, ClaimsPrincipal user)
        {
            Order order = dbHelper.Order.FindById(id);
            Player player = dbHelper.Player.FindById(order.PlayerId);
            Team team = dbHelper.Team.FindById(player.TeamId);

            authorizationHelper.CheckPlayerIsInTheSameTeamAsResource(user, team.TeamId);
            
            OrderDto orderDto = mapper.Map<OrderDto>(order);

            return orderDto;
        }

        public OrderDto Create(PostOrderDto orderDto, ClaimsPrincipal user)
        {
            Player targetPlayer = dbHelper.Player.FindById(orderDto.PlayerId);
            
            if (targetPlayer.TeamId is null) throw new ForbidException("Can't kick player from team that is not in a team.");
            
            Team team = dbHelper.Team.FindById(targetPlayer.TeamId);
            
            authorizationHelper.CheckPlayerOwnsResource(user, team.OfficerPlayerId);
            
            Room room = dbHelper.Room.FindByIdIncludingBattle(targetPlayer.RoomId);

            if (room.Battle is null) throw new ForbidException("Can't create order because there is no battle.");
            
            Location location = mapper.Map<Location>(orderDto);
            dbContext.Location.Add(location);

            dbContext.SaveChanges();
            
            Order order = new Order();
            order.LocationId = location.LocationId;
            order.PlayerId = targetPlayer.PlayerId;
            order.BattleId = room.Battle.BattleId;
            order.OrderType = orderDto.Type;
            dbContext.Order.Add(order);

            dbContext.SaveChanges();

            return mapper.Map<OrderDto>(order);
        }
        
        public void DeleteById(int id, ClaimsPrincipal user)
        {
            Order order = dbHelper.Order.FindById(id);
            Player player = dbHelper.Player.FindById(order.PlayerId);
            Team team = dbHelper.Team.FindById(player.TeamId);

            authorizationHelper.CheckPlayerOwnsResource(user, team.OfficerPlayerId);
            
            dbContext.Order.Remove(order);
            dbContext.SaveChanges();
        }
    }
}
