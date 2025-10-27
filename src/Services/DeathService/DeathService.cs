using System.Security.Claims;
using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Death;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Models.Helpers;
using AirsoftBattlefieldManagementSystemAPI.Realtime;
using AirsoftBattlefieldManagementSystemAPI.Services.AuthorizationHelperService;
using AirsoftBattlefieldManagementSystemAPI.Services.ClaimsHelperService;
using AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;

namespace AirsoftBattlefieldManagementSystemAPI.Services.DeathService
{
    public class DeathService(
        IMapper mapper, 
        IBattleManagementSystemDbContext dbContext, 
        IClaimsHelperService claimsHelper, 
        IAuthorizationHelperService authorizationHelper, 
        IDbContextHelperService dbHelper,
        IHubContext<RoomNotificationHub, IRoomNotificationClient> hubContext
        ) : IDeathService
    {
        public DeathDto GetById(int id, ClaimsPrincipal user)
        {
            Death death = dbHelper.Death.FindByIdIncludingBattle(id);

            authorizationHelper.CheckPlayerIsInTheSameRoomAsResource(user, death.Battle.RoomId);
            
            DeathDto deathDto = mapper.Map<DeathDto>(death);

            return deathDto;
        }

        public List<DeathDto> GetAllOfPlayerWithId(int playerId, ClaimsPrincipal user)
        {
            Player player = dbHelper.Player.FindById(playerId);
            
            authorizationHelper.CheckPlayerIsInTheSameRoomAsResource(user, player.RoomId);
            
            var deaths = dbHelper.Death.FindAllOfPlayer(player);

            List<DeathDto> deathDtos = deaths.Select(location =>
            {
                return mapper.Map<DeathDto>(location, opt =>
                {
                    opt.Items["playerId"] = playerId;
                });
            }).ToList();

            return deathDtos;
        }

        public DeathDto Create(PostDeathDto deathDto, ClaimsPrincipal user)
        {
            int playerId = claimsHelper.GetIntegerClaimValue("playerId", user);
            Player player = dbHelper.Player.FindById(playerId);
            Room room = dbHelper.Room.FindByIdIncludingRelated(player.RoomId);

            Location location = mapper.Map<Location>(deathDto);
            dbContext.Location.Add(location);
            
            dbContext.SaveChanges();
            
            Death death = new();
            death.LocationId = location.LocationId;
            death.PlayerId = playerId;
            death.BattleId = room.Battle?.BattleId ?? 0;
            dbContext.Death.Add(death);

            dbContext.SaveChanges();
            
            DeathDto responseDeathDto = mapper.Map<DeathDto>(death);            
            
            IEnumerable<string> playerIds = room.GetAllPlayerIdsWithoutSelf(player.PlayerId);

            hubContext.Clients.Users(playerIds).DeathCreated(responseDeathDto);

            return responseDeathDto;
        }

        public DeathDto Update(int id, PutDeathDto deathDto, ClaimsPrincipal user)
        {
            Death death = dbHelper.Death.FindById(id);
            Player player = dbHelper.Player.FindById(death.PlayerId);
            Room room = dbHelper.Room.FindByIdIncludingPlayers(player.RoomId);
            
            authorizationHelper.CheckPlayerOwnsResource(user, death.PlayerId);
            
            mapper.Map(deathDto, death.Location);
            dbContext.Location.Update(death.Location);
            dbContext.SaveChanges();
            
            DeathDto responseDeathDto = mapper.Map<DeathDto>(death);            
            
            IEnumerable<string> playerIds = room.GetAllPlayerIdsWithoutSelf(player.PlayerId);

            hubContext.Clients.Users(playerIds).DeathUpdated(responseDeathDto);
            
            return responseDeathDto;
        }

        public void DeleteById(int id, ClaimsPrincipal user)
        {
            Death death = dbHelper.Death.FindById(id);
            Player player = dbHelper.Player.FindById(death.PlayerId);
            Room room = dbHelper.Room.FindByIdIncludingPlayers(player.RoomId);
            
            authorizationHelper.CheckPlayerOwnsResource(user, death.PlayerId);

            dbContext.Death.Remove(death);
            dbContext.SaveChanges();
            
            IEnumerable<string> playerIds = room.GetAllPlayerIdsWithoutSelf(player.PlayerId);

            hubContext.Clients.Users(playerIds).DeathDeleted(id);
        }
    }
}
