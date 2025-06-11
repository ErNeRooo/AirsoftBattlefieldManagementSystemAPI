using System.Security.Claims;
using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Battle;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Services.AuthorizationHelperService;
using AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService;
using AutoMapper;

namespace AirsoftBattlefieldManagementSystemAPI.Services.BattleService
{
    public class BattleService(IMapper mapper, IBattleManagementSystemDbContext dbContext, IAuthorizationHelperService authorizationHelper, IDbContextHelperService dbHelper) : IBattleService
    {
        public BattleDto GetById(int id, ClaimsPrincipal user)
        {
            Battle battle = dbHelper.FindBattleById(id);

            authorizationHelper.CheckPlayerIsInTheSameRoomAsResource(user, battle.RoomId);
            
            BattleDto battleDto = mapper.Map<BattleDto>(battle);

            return battleDto;
        }

        public BattleDto Create(PostBattleDto battleDto, ClaimsPrincipal user)
        {
            Room room = dbHelper.FindRoomById(battleDto.RoomId);
            
            authorizationHelper.CheckPlayerOwnsResource(user, room.AdminPlayerId);
            
            Battle battle = mapper.Map<Battle>(battleDto);
            dbContext.Battle.Add(battle);
            dbContext.SaveChanges();
            
            return mapper.Map<BattleDto>(battle);
        }

        public BattleDto Update(int id, PutBattleDto battleDto, ClaimsPrincipal user)
        {
            Battle previousBattle = dbHelper.FindBattleById(id);
            
            Room room = dbHelper.FindRoomById(previousBattle.RoomId);
            
            authorizationHelper.CheckPlayerOwnsResource(user, room.AdminPlayerId);
            
            mapper.Map(battleDto, previousBattle);
            dbContext.Battle.Update(previousBattle);
            dbContext.SaveChanges();
            
            return mapper.Map<BattleDto>(previousBattle);
        }

        public void Delete(int id, ClaimsPrincipal user)
        {
            Battle battle = dbHelper.FindBattleById(id);

            Room room = dbHelper.FindRoomById(battle.RoomId);
            
            authorizationHelper.CheckPlayerOwnsResource(user, room.AdminPlayerId);
            
            dbContext.Battle.Remove(battle);
            dbContext.SaveChanges();
        }
    }
}
