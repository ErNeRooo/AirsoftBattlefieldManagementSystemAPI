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
    public class DeathService(IMapper mapper, IBattleManagementSystemDbContext dbContext, IAuthorizationService authorizationService) : IDeathService
    {
        public DeathDto? GetById(int id)
        {
            Death? death = dbContext.Death.Include(k=> k.Location).FirstOrDefault(t => t.DeathId == id);

            if (death is null) throw new NotFoundException($"Death with id {id} not found");

            DeathDto deathDto = mapper.Map<DeathDto>(death);

            return deathDto;
        }

        public List<DeathDto>? GetAllOfPlayerWithId(int playerId)
        {
            var deaths = dbContext.Death.Include(k => k.Location)
                .Where(k => k.PlayerId == playerId).ToList();

            if (deaths is null) throw new NotFoundException($"Player with id {playerId} not found");

            List<DeathDto> deathDtos = deaths.Select(location =>
            {
                return mapper.Map<DeathDto>(location, opt =>
                {
                    opt.Items["playerId"] = playerId;
                });
            }).ToList();

            return deathDtos;
        }

        public int Create(int playerId, PostDeathDto deathDto, ClaimsPrincipal user)
        {
            var authorizationResult =
                authorizationService.AuthorizeAsync(user, playerId,
                    new PlayerOwnsResourceRequirement()).Result;

            if (!authorizationResult.Succeeded) throw new ForbidException($"You're unauthorize to manipulate this resource");
            
            Player? player = dbContext.Player.FirstOrDefault(p => p.PlayerId == playerId);

            if(player is null) throw new NotFoundException($"Player with id {playerId} not found");

            Location location = mapper.Map<Location>(deathDto);
            dbContext.Location.Add(location);
            
            dbContext.SaveChanges();
            
            Death death = new Death();
            death.LocationId = location.LocationId;
            death.PlayerId = playerId;
            dbContext.Death.Add(death);

            dbContext.SaveChanges();

            return death.DeathId;
        }

        public void Update(int id, PutDeathDto deathDto, ClaimsPrincipal user)
        {
            Death? previousDeath = dbContext.Death
                .Include(k => k.Location)
                .FirstOrDefault(t => t.DeathId == id);

            if (previousDeath is null) throw new NotFoundException($"Death with id {id} not found");
            
            var authorizationResult =
                authorizationService.AuthorizeAsync(user, previousDeath.PlayerId,
                    new PlayerOwnsResourceRequirement()).Result;

            if (!authorizationResult.Succeeded) throw new ForbidException($"You're unauthorize to manipulate this resource");
            
            mapper.Map(deathDto, previousDeath.Location);
            dbContext.Location.Update(previousDeath.Location);
            dbContext.SaveChanges();
        }

        public void DeleteById(int id, ClaimsPrincipal user)
        {
            Death? death = dbContext.Death.FirstOrDefault(t => t.DeathId == id);

            if(death is null) throw new NotFoundException($"Death with id {id} not found");

            var authorizationResult =
                authorizationService.AuthorizeAsync(user, death.PlayerId,
                    new PlayerOwnsResourceRequirement()).Result;

            if (!authorizationResult.Succeeded) throw new ForbidException($"You're unauthorize to manipulate this resource");
            
            dbContext.Death.Remove(death);
            dbContext.SaveChanges();
        }
    }
}
