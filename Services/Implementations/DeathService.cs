using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Services.Abstractions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace AirsoftBattlefieldManagementSystemAPI.Services.Implementations
{
    public class DeathService(IMapper mapper, IBattleManagementSystemDbContext dbContext) : IDeathService
    {
        public DeathDto? GetById(int id)
        {
            Death? death = dbContext.Death.Include(k=> k.Location).FirstOrDefault(t => t.DeathId == id);

            if (death is null) return null;

            DeathDto deathDto = mapper.Map<DeathDto>(death);

            return deathDto;
        }

        public List<DeathDto>? GetAllOfPlayerWithId(int playerId)
        {
            var deaths = dbContext.Death.Include(k => k.Location)
                .Where(k => k.PlayerId == playerId).ToList();

            if (deaths.IsNullOrEmpty()) return null;

            List<DeathDto> deathDtos = deaths.Select(location =>
            {
                return mapper.Map<DeathDto>(location, opt =>
                {
                    opt.Items["playerId"] = playerId;
                });
            }).ToList();

            return deathDtos;
        }

        public int? Create(int playerId, CreateDeathDto deathDto)
        {
            Player? player = dbContext.Player.FirstOrDefault(p => p.PlayerId == playerId);

            if(player is null) return null;

            Location location = mapper.Map<Location>(deathDto);
            dbContext.Location.Add(location);

            dbContext.SaveChanges();

            Death death = new Death();
            death.LocationId = location.LocationId;
            death.PlayerId = playerId;
            dbContext.Death.Add(death);

            dbContext.SaveChanges();

            return location.LocationId;
        }

        public bool Update(int id, UpdateDeathDto deathDto)
        {
            Death? previousDeath = dbContext.Death
                .Include(k => k.Location)
                .FirstOrDefault(t => t.DeathId == id);

            if (previousDeath is null) return false;

            mapper.Map(deathDto, previousDeath.Location);
            dbContext.Location.Update(previousDeath.Location);
            dbContext.SaveChanges();

            return true;
        }

        public bool DeleteById(int id)
        {
            Death? death = dbContext.Death.FirstOrDefault(t => t.DeathId == id);

            if(death is null) return false;

            dbContext.Death.Remove(death);
            dbContext.SaveChanges();

            return true;
        }
    }
}
