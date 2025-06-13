using System.Security.Claims;
using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Death;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Services.AuthorizationHelperService;
using AirsoftBattlefieldManagementSystemAPI.Services.ClaimsHelperService;
using AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService;
using AutoMapper;

namespace AirsoftBattlefieldManagementSystemAPI.Services.DeathService
{
    public class DeathService(IMapper mapper, IBattleManagementSystemDbContext dbContext, IClaimsHelperService claimsHelper, IAuthorizationHelperService authorizationHelper, IDbContextHelperService dbHelper) : IDeathService
    {
        public DeathDto GetById(int id, ClaimsPrincipal user)
        {
            Death death = dbHelper.Death.FindById(id);

            authorizationHelper.CheckPlayerIsInTheSameRoomAsResource(user, death.RoomId);
            
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

            Location location = mapper.Map<Location>(deathDto);
            dbContext.Location.Add(location);
            
            dbContext.SaveChanges();
            
            Death death = new Death();
            death.LocationId = location.LocationId;
            death.PlayerId = playerId;
            death.RoomId = (int)player.RoomId;
            dbContext.Death.Add(death);

            dbContext.SaveChanges();

            return mapper.Map<DeathDto>(death);
        }

        public DeathDto Update(int id, PutDeathDto deathDto, ClaimsPrincipal user)
        {
            Death previousDeath = dbHelper.Death.FindById(id);
            
            authorizationHelper.CheckPlayerOwnsResource(user, previousDeath.PlayerId);
            
            mapper.Map(deathDto, previousDeath.Location);
            dbContext.Location.Update(previousDeath.Location);
            dbContext.SaveChanges();
            
            return mapper.Map<DeathDto>(previousDeath);
        }

        public void DeleteById(int id, ClaimsPrincipal user)
        {
            Death death = dbHelper.Death.FindById(id);
            
            authorizationHelper.CheckPlayerOwnsResource(user, death.PlayerId);

            dbContext.Death.Remove(death);
            dbContext.SaveChanges();
        }
    }
}
