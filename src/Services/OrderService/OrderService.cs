using System.Security.Claims;
using AirsoftBattlefieldManagementSystemAPI.Exceptions;
using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Order;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Models.Helpers;
using AirsoftBattlefieldManagementSystemAPI.Realtime;
using AirsoftBattlefieldManagementSystemAPI.Services.AuthorizationHelperService;
using AirsoftBattlefieldManagementSystemAPI.Services.ClaimsHelperService;
using AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;

namespace AirsoftBattlefieldManagementSystemAPI.Services.OrderService
{
    public class OrderService(
        IMapper mapper, 
        IBattleManagementSystemDbContext dbContext, 
        IAuthorizationHelperService authorizationHelper, 
        IClaimsHelperService claimsHelper,
        IDbContextHelperService dbHelper, 
        IHubContext<RoomNotificationHub, IRoomNotificationClient> hubContext
        ) : IOrderService
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
            int senderPlayerId = claimsHelper.GetIntegerClaimValue("playerId", user);
            Player targetPlayer = dbHelper.Player.FindById(orderDto.PlayerId);
            
            if (targetPlayer.TeamId is null) throw new ForbidException("Can't order a player that is not in a team.");
            
            Team team = dbHelper.Team.FindById(targetPlayer.TeamId);
            
            authorizationHelper.CheckPlayerOwnsResource(user, team.OfficerPlayerId);
            
            Room room = dbHelper.Room.FindByIdIncludingRelated(targetPlayer.RoomId);

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
            
            OrderDto responseOrderDto = mapper.Map<OrderDto>(order);            
            
            IEnumerable<string> playerIds = room.GetTeamPlayerIdsWithoutSelf(team.TeamId, senderPlayerId);

            hubContext.Clients.Users(playerIds).OrderCreated(responseOrderDto);

            return mapper.Map<OrderDto>(order);
        }
        
        public void DeleteById(int id, ClaimsPrincipal user)
        {
            int senderPlayerId = claimsHelper.GetIntegerClaimValue("playerId", user);
            
            Order order = dbHelper.Order.FindById(id);
            Player player = dbHelper.Player.FindById(order.PlayerId);
            Team team = dbHelper.Team.FindById(player.TeamId);
            Room room = dbHelper.Room.FindByIdIncludingPlayers(player.RoomId);
            
            authorizationHelper.CheckPlayerOwnsResource(user, team.OfficerPlayerId);
            
            dbContext.Order.Remove(order);
            dbContext.SaveChanges();
            
            IEnumerable<string> playerIds = room.GetTeamPlayerIdsWithoutSelf(team.TeamId, senderPlayerId);

            hubContext.Clients.Users(playerIds).OrderDeleted(id);
        }
    }
}
