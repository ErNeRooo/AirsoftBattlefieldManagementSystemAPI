using System.Security.Claims;
using AirsoftBattlefieldManagementSystemAPI.Authorization;
using AirsoftBattlefieldManagementSystemAPI.Exceptions;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Services.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace AirsoftBattlefieldManagementSystemAPI.Services.Implementations
{
    public class KillService(IMapper mapper, IBattleManagementSystemDbContext dbContext, IAuthorizationService authorizationService) : IKillService
    {
        public KillDto? GetById(int id)
        {
            Kill? kill = dbContext.Kill.Include(k=> k.Location).FirstOrDefault(t => t.KillId == id);

            if (kill is null) throw new NotFoundException($"Kill with id {id} not found");

            KillDto killDto = mapper.Map<KillDto>(kill);

            return killDto;
        }

        public List<KillDto> GetAllOfPlayerWithId(int playerId)
        {
            var kills = dbContext.Kill.Include(k => k.Location)
                .Where(k => k.PlayerId == playerId).ToList();

            if (kills is null) throw new NotFoundException($"Player with id {playerId} not found");

            List<KillDto> killDtos = kills.Select(location =>
            {
                return mapper.Map<KillDto>(location, opt =>
                {
                    opt.Items["playerId"] = playerId;
                });
            }).ToList();

            return killDtos;
        }

        public int Create(int playerId, PostKillDto killDto, ClaimsPrincipal user)
        {
            var authorizationResult =
                authorizationService.AuthorizeAsync(user, playerId,
                    new PlayerOwnsResourceRequirement()).Result;

            if (!authorizationResult.Succeeded) throw new ForbidException($"You're unauthorize to manipulate this resource");

            Player? player = dbContext.Player.FirstOrDefault(p => p.PlayerId == playerId);

            if(player is null) throw new NotFoundException($"Player with id {playerId} not found");

            Location location = mapper.Map<Location>(killDto);
            dbContext.Location.Add(location);

            dbContext.SaveChanges();
            
            Kill kill = new Kill();
            kill.LocationId = location.LocationId;
            kill.PlayerId = playerId;
            dbContext.Kill.Add(kill);

            dbContext.SaveChanges();

            return kill.KillId;
        }

        public void Update(int id, PutKillDto killDto, ClaimsPrincipal user)
        {
            Kill? previousKill = dbContext.Kill
                .Include(k => k.Location)
                .FirstOrDefault(t => t.KillId == id);

            if (previousKill is null) throw new NotFoundException($"Kill with id {id} not found");

            var authorizationResult =
                authorizationService.AuthorizeAsync(user, previousKill.PlayerId,
                    new PlayerOwnsResourceRequirement()).Result;

            if (!authorizationResult.Succeeded) throw new ForbidException($"You're unauthorize to manipulate this resource");
            
            Location updatedLocation = mapper.Map(killDto, previousKill.Location);
            dbContext.Location.Update(updatedLocation);
            dbContext.SaveChanges();
        }

        public void DeleteById(int id, ClaimsPrincipal user)
        {
            Kill? kill = dbContext.Kill.FirstOrDefault(t => t.KillId == id);

            if(kill is null) throw new NotFoundException($"Kill with id {id} not found");

            var authorizationResult =
                authorizationService.AuthorizeAsync(user, kill.PlayerId,
                    new PlayerOwnsResourceRequirement()).Result;

            if (!authorizationResult.Succeeded) throw new ForbidException($"You're unauthorize to manipulate this resource");
            
            dbContext.Kill.Remove(kill);
            dbContext.SaveChanges();
        }
    }
}
