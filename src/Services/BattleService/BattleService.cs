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
            Battle battle = dbHelper.Battle.FindByIdIncludingRoom(id);

            authorizationHelper.CheckPlayerIsInTheSameRoomAsResource(user, battle.Room.RoomId);
            
            BattleDto battleDto = mapper.Map<BattleDto>(battle);

            return battleDto;
        }

        public BattleDto Create(PostBattleDto battleDto, ClaimsPrincipal user)
        {
            Room room = dbHelper.Room.FindById(battleDto.RoomId);
            
            authorizationHelper.CheckPlayerOwnsResource(user, room.AdminPlayerId);
            
            Battle battle = mapper.Map<Battle>(battleDto);
            dbContext.Battle.Add(battle);
            dbContext.SaveChanges();
            
            return mapper.Map<BattleDto>(battle);
        }

        public BattleDto Update(int id, PutBattleDto battleDto, ClaimsPrincipal user)
        {
            Battle previousBattle = dbHelper.Battle.FindByIdIncludingRoom(id);
            
            authorizationHelper.CheckPlayerOwnsResource(user, previousBattle.Room.AdminPlayerId);
            
            mapper.Map(battleDto, previousBattle);
            dbContext.Battle.Update(previousBattle);
            dbContext.SaveChanges();
            
            return mapper.Map<BattleDto>(previousBattle);
        }

        public void Delete(int id, ClaimsPrincipal user)
        {
            Battle battle = dbHelper.Battle.FindByIdIncludingRoom(id);
            
            authorizationHelper.CheckPlayerOwnsResource(user, battle.Room.AdminPlayerId);
            
            dbContext.Battle.Remove(battle);
            dbContext.SaveChanges();
        }
    }
}
