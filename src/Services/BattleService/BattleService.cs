using System.Security.Claims;
using AirsoftBattlefieldManagementSystemAPI.Exceptions;
using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Battle;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Models.Helpers;
using AirsoftBattlefieldManagementSystemAPI.Realtime;
using AirsoftBattlefieldManagementSystemAPI.Services.AuthorizationHelperService;
using AirsoftBattlefieldManagementSystemAPI.Services.ClaimsHelperService;
using AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;

namespace AirsoftBattlefieldManagementSystemAPI.Services.BattleService
{
    public class BattleService(
        IMapper mapper, 
        IBattleManagementSystemDbContext dbContext, 
        IAuthorizationHelperService authorizationHelper, 
        IDbContextHelperService dbHelper,
        IClaimsHelperService claimsHelper,
        IHubContext<RoomNotificationHub, IRoomNotificationClient> hubContext) : IBattleService
    {
        public BattleDto GetById(int id, ClaimsPrincipal user)
        {
            Battle battle = dbHelper.Battle.FindByIdIncludingRoom(id);

            authorizationHelper.CheckPlayerIsInTheSameRoomAsResource(user, battle.Room.RoomId);
            
            BattleDto battleDto = mapper.Map<BattleDto>(battle);

            return battleDto;
        }

        public BattleDto Create(PostBattleDto postBattleDto, ClaimsPrincipal user)
        {
            int playerId = claimsHelper.GetIntegerClaimValue("playerId", user);
            Room room = dbHelper.Room.FindByIdIncludingRelated(postBattleDto.RoomId);
            
            authorizationHelper.CheckPlayerOwnsResource(user, room.AdminPlayerId);
            if(room.Battle is not null) throw new BadRequestException("There is already a battle in this room.");
            
            Battle battle = mapper.Map<Battle>(postBattleDto);
            dbContext.Battle.Add(battle);
            dbContext.SaveChanges();

            BattleDto responseBattleDto = mapper.Map<BattleDto>(battle);            
            
            IEnumerable<string> playerIds = room.GetAllPlayerIdsWithoutSelf(playerId);

            hubContext.Clients.Users(playerIds).BattleCreated(responseBattleDto);
            
            return responseBattleDto;
        }

        public BattleDto Update(int id, PutBattleDto battleDto, ClaimsPrincipal user)
        {
            int playerId = claimsHelper.GetIntegerClaimValue("playerId", user);
            Battle battle = dbHelper.Battle.FindByIdIncludingRoom(id);
            Room room = dbHelper.Room.FindByIdIncludingPlayers(battle.RoomId);
            
            authorizationHelper.CheckPlayerOwnsResource(user, battle.Room.AdminPlayerId);
            
            mapper.Map(battleDto, battle);
            dbContext.Battle.Update(battle);
            dbContext.SaveChanges();
            
            BattleDto responseBattleDto = mapper.Map<BattleDto>(battle);            
            
            IEnumerable<string> playerIds = room.GetAllPlayerIdsWithoutSelf(playerId);

            hubContext.Clients.Users(playerIds).BattleUpdated(responseBattleDto);
            
            return responseBattleDto;
        }

        public void Delete(int id, ClaimsPrincipal user)
        {
            int playerId = claimsHelper.GetIntegerClaimValue("playerId", user);
            Battle battle = dbHelper.Battle.FindByIdIncludingRoom(id);
            Room room = dbHelper.Room.FindByIdIncludingPlayers(battle.RoomId);
            
            authorizationHelper.CheckPlayerOwnsResource(user, battle.Room.AdminPlayerId);
            
            dbContext.Battle.Remove(battle);
            dbContext.SaveChanges();
            
            IEnumerable<string> playerIds = room.GetAllPlayerIdsWithoutSelf(playerId);

            hubContext.Clients.Users(playerIds).BattleDeleted(id);
        }
    }
}
