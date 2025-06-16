using System.Security.Claims;
using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Kill;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Services.AuthorizationHelperService;
using AirsoftBattlefieldManagementSystemAPI.Services.ClaimsHelperService;
using AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService;
using AutoMapper;

namespace AirsoftBattlefieldManagementSystemAPI.Services.KillService
{
    public class KillService(IMapper mapper, IBattleManagementSystemDbContext dbContext, IAuthorizationHelperService authorizationHelper, IDbContextHelperService dbHelper, IClaimsHelperService claimsHelper) : IKillService
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
            Room room = dbHelper.Room.FindByIdIncludingBattle(player.RoomId);
            
            authorizationHelper.CheckPlayerOwnsResource(user, playerId);
            
            Location location = mapper.Map<Location>(killDto);
            dbContext.Location.Add(location);

            dbContext.SaveChanges();
            
            Kill kill = new Kill();
            kill.LocationId = location.LocationId;
            kill.PlayerId = playerId;
            kill.BattleId = room.Battle.BattleId;
            dbContext.Kill.Add(kill);

            dbContext.SaveChanges();

            return mapper.Map<KillDto>(kill);
        }

        public KillDto Update(int id, PutKillDto killDto, ClaimsPrincipal user)
        {
            Kill previousKill = dbHelper.Kill.FindById(id);

            authorizationHelper.CheckPlayerOwnsResource(user, previousKill.PlayerId);
            
            Location updatedLocation = mapper.Map(killDto, previousKill.Location);
            dbContext.Location.Update(updatedLocation);
            dbContext.SaveChanges();
            
            return mapper.Map<KillDto>(previousKill);   
        }

        public void DeleteById(int id, ClaimsPrincipal user)
        {
            Kill kill = dbHelper.Kill.FindById(id);

            authorizationHelper.CheckPlayerOwnsResource(user, kill.PlayerId);
            
            dbContext.Kill.Remove(kill);
            dbContext.SaveChanges();
        }
    }
}
