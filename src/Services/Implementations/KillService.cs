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
    public class KillService(IMapper mapper, IBattleManagementSystemDbContext dbContext) : IKillService
    {
        public KillDto? GetById(int id)
        {
            Kill? kill = dbContext.Kill.Include(k=> k.Location).FirstOrDefault(t => t.KillId == id);

            if (kill is null) return null;

            KillDto killDto = mapper.Map<KillDto>(kill);

            return killDto;
        }

        public List<KillDto>? GetAllOfPlayerWithId(int playerId)
        {
            var kills = dbContext.Kill.Include(k => k.Location)
                .Where(k => k.PlayerId == playerId).ToList();

            if (kills.IsNullOrEmpty()) return null;

            List<KillDto> killDtos = kills.Select(location =>
            {
                return mapper.Map<KillDto>(location, opt =>
                {
                    opt.Items["playerId"] = playerId;
                });
            }).ToList();

            return killDtos;
        }

        public int? Create(int playerId, CreateKillDto killDto)
        {
            Player? player = dbContext.Player.FirstOrDefault(p => p.PlayerId == playerId);

            if(player is null) return null;

            Location location = mapper.Map<Location>(killDto);
            dbContext.Location.Add(location);

            Kill kill = new Kill();
            kill.LocationId = location.LocationId;
            kill.PlayerId = playerId;
            dbContext.Kill.Add(kill);

            dbContext.SaveChanges();

            return kill.KillId;
        }

        public bool Update(int id, UpdateKillDto killDto)
        {
            Kill? previousKill = dbContext.Kill
                .Include(k => k.Location)
                .FirstOrDefault(t => t.KillId == id);

            if (previousKill is null) return false;

            Location updatedLocation = mapper.Map(killDto, previousKill.Location);
            dbContext.Location.Update(updatedLocation);
            dbContext.SaveChanges();

            return true;
        }

        public bool DeleteById(int id)
        {
            Kill? kill = dbContext.Kill.FirstOrDefault(t => t.KillId == id);

            if(kill is null) return false;

            dbContext.Kill.Remove(kill);
            dbContext.SaveChanges();

            return true;
        }
    }
}
