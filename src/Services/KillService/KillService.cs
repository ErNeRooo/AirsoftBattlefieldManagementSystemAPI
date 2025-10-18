using System.Security.Claims;
using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Kill;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Models.Helpers;
using AirsoftBattlefieldManagementSystemAPI.Realtime;
using AirsoftBattlefieldManagementSystemAPI.Services.AuthorizationHelperService;
using AirsoftBattlefieldManagementSystemAPI.Services.ClaimsHelperService;
using AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;

namespace AirsoftBattlefieldManagementSystemAPI.Services.KillService
{
    public class KillService(
        IMapper mapper, 
        IBattleManagementSystemDbContext dbContext, 
        IAuthorizationHelperService authorizationHelper, 
        IDbContextHelperService dbHelper, 
        IClaimsHelperService claimsHelper,
        IHubContext<RoomNotificationHub, IRoomNotificationClient> hubContext
        ) : IKillService
    {
        public KillDto GetById(int id, ClaimsPrincipal user)
        {
            Kill kill = dbHelper.Kill.FindByIdIncludingBattle(id);
            
            authorizationHelper.CheckPlayerIsInTheSameRoomAsResource(user, kill.Battle.RoomId);
            
            KillDto killDto = mapper.Map<KillDto>(kill);

            return killDto;
        }

        public List<KillDto> GetAllOfPlayerWithId(int playerId, ClaimsPrincipal user)
        {
            Player player = dbHelper.Player.FindById(playerId);
            
            authorizationHelper.CheckPlayerIsInTheSameRoomAsResource(user, player.RoomId);
            
            var kills = dbHelper.Kill.FindAllOfPlayer(player);

            List<KillDto> killDtos = kills.Select(kill =>
            {
                return mapper.Map<KillDto>(kill, opt =>
                {
                    opt.Items["playerId"] = playerId;
                });
            }).ToList();

            return killDtos;
        }

        public KillDto Create(PostKillDto killDto, ClaimsPrincipal user)
        {
            int playerId = claimsHelper.GetIntegerClaimValue("playerId", user);
            Player player = dbHelper.Player.FindById(playerId);
            Room room = dbHelper.Room.FindByIdIncludingRelated(player.RoomId);
            
            Location location = mapper.Map<Location>(killDto);
            dbContext.Location.Add(location);

            dbContext.SaveChanges();
            
            Kill kill = new Kill();
            kill.LocationId = location.LocationId;
            kill.PlayerId = playerId;
            kill.BattleId = room.Battle.BattleId;
            dbContext.Kill.Add(kill);

            dbContext.SaveChanges();
            
            KillDto responseKillDto = mapper.Map<KillDto>(kill);            
            
            IEnumerable<string> playerIds = room.GetAllPlayerIdsWithoutSelf(player.PlayerId);

            hubContext.Clients.Users(playerIds).KillCreated(responseKillDto);

            return responseKillDto;
        }

        public KillDto Update(int id, PutKillDto killDto, ClaimsPrincipal user)
        {
            Kill kill = dbHelper.Kill.FindById(id);
            Player player = dbHelper.Player.FindById(kill.PlayerId);
            Room room = dbHelper.Room.FindByIdIncludingPlayers(player.RoomId);

            authorizationHelper.CheckPlayerOwnsResource(user, kill.PlayerId);
            
            Location updatedLocation = mapper.Map(killDto, kill.Location);
            dbContext.Location.Update(updatedLocation);
            dbContext.SaveChanges();
            
            KillDto responseKillDto = mapper.Map<KillDto>(kill);            
            
            IEnumerable<string> playerIds = room.GetAllPlayerIdsWithoutSelf(player.PlayerId);

            hubContext.Clients.Users(playerIds).KillUpdated(responseKillDto);
            
            return mapper.Map<KillDto>(kill);   
        }

        public void DeleteById(int id, ClaimsPrincipal user)
        {
            Kill kill = dbHelper.Kill.FindById(id);
            Player player = dbHelper.Player.FindById(kill.PlayerId);
            Room room = dbHelper.Room.FindByIdIncludingPlayers(player.RoomId);

            authorizationHelper.CheckPlayerOwnsResource(user, kill.PlayerId);
            
            dbContext.Kill.Remove(kill);
            dbContext.SaveChanges();
            
            IEnumerable<string> playerIds = room.GetAllPlayerIdsWithoutSelf(player.PlayerId);

            hubContext.Clients.Users(playerIds).KillDeleted(id);
        }
    }
}
