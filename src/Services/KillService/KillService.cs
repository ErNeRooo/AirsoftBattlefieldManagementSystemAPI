using System.Security.Claims;
using AirsoftBattlefieldManagementSystemAPI.Authorization;
using AirsoftBattlefieldManagementSystemAPI.Exceptions;
using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Kill;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace AirsoftBattlefieldManagementSystemAPI.Services.KillService
{
    public class KillService(IMapper mapper, IBattleManagementSystemDbContext dbContext, IAuthorizationService authorizationService) : IKillService
    {
        public KillDto GetById(int id, ClaimsPrincipal user)
        {
            Kill? kill = dbContext.Kill.Include(k=> k.Location).FirstOrDefault(t => t.KillId == id);

            if (kill is null) throw new NotFoundException($"Kill with id {id} not found");
            
            var playerIsInTheSameRoomAsResourceResult =
                authorizationService.AuthorizeAsync(user, kill.RoomId,
                    new PlayerIsInTheSameRoomAsResourceRequirement()).Result;

            if (!playerIsInTheSameRoomAsResourceResult.Succeeded) throw new ForbidException($"You're unauthorize to manipulate this resource");
            
            KillDto killDto = mapper.Map<KillDto>(kill);

            return killDto;
        }

        public List<KillDto> GetAllOfPlayerWithId(int playerId, ClaimsPrincipal user)
        {
            Player? player = dbContext.Player.FirstOrDefault(p => p.PlayerId == playerId);
            
            if (player is null) throw new NotFoundException($"Player with id {playerId} not found");
            
            var playerIsInTheSameRoomAsResourceResult =
                authorizationService.AuthorizeAsync(user, player.RoomId,
                    new PlayerIsInTheSameRoomAsResourceRequirement()).Result;

            if (!playerIsInTheSameRoomAsResourceResult.Succeeded) throw new ForbidException($"You're unauthorize to manipulate this resource");
            
            var kills = dbContext.Kill
                .Include(k => k.Location)
                .Where(kill => kill.PlayerId == playerId && kill.RoomId == player.RoomId).ToList();

            List<KillDto> killDtos = kills.Select(location =>
            {
                return mapper.Map<KillDto>(location, opt =>
                {
                    opt.Items["playerId"] = playerId;
                });
            }).ToList();

            return killDtos;
        }

        public KillDto Create(int playerId, PostKillDto killDto, ClaimsPrincipal user)
        {
            Player? player = dbContext.Player.FirstOrDefault(p => p.PlayerId == playerId);

            if(player is null) throw new NotFoundException($"Player with id {playerId} not found");
            
            var playerOwnsResourceResult =
                authorizationService.AuthorizeAsync(user, player.PlayerId,
                    new PlayerOwnsResourceRequirement()).Result;
            
            var playerIsInTheSameRoomAsResourceResult =
                authorizationService.AuthorizeAsync(user, player.RoomId,
                    new PlayerIsInTheSameRoomAsResourceRequirement()).Result;

            if (!playerOwnsResourceResult.Succeeded || !playerIsInTheSameRoomAsResourceResult.Succeeded) throw new ForbidException($"You're unauthorize to manipulate this resource");
            
            Location location = mapper.Map<Location>(killDto);
            dbContext.Location.Add(location);

            dbContext.SaveChanges();
            
            Kill kill = new Kill();
            kill.LocationId = location.LocationId;
            kill.PlayerId = playerId;
            kill.RoomId = (int)player.RoomId;
            dbContext.Kill.Add(kill);

            dbContext.SaveChanges();

            return mapper.Map<KillDto>(kill);
        }

        public KillDto Update(int id, PutKillDto killDto, ClaimsPrincipal user)
        {
            Kill? previousKill = dbContext.Kill
                .Include(k => k.Location)
                .FirstOrDefault(t => t.KillId == id);

            if (previousKill is null) throw new NotFoundException($"Kill with id {id} not found");

            var playerOwnsResourceResult =
                authorizationService.AuthorizeAsync(user, previousKill.PlayerId,
                    new PlayerOwnsResourceRequirement()).Result;
            
            var playerIsInTheSameRoomAsResourceResult =
                authorizationService.AuthorizeAsync(user, previousKill.RoomId,
                    new PlayerIsInTheSameRoomAsResourceRequirement()).Result;

            if (!playerOwnsResourceResult.Succeeded || !playerIsInTheSameRoomAsResourceResult.Succeeded) throw new ForbidException($"You're unauthorize to manipulate this resource");

            Location updatedLocation = mapper.Map(killDto, previousKill.Location);
            dbContext.Location.Update(updatedLocation);
            dbContext.SaveChanges();
            
            return mapper.Map<KillDto>(previousKill);   
        }

        public void DeleteById(int id, ClaimsPrincipal user)
        {
            Kill? kill = dbContext.Kill.FirstOrDefault(t => t.KillId == id);

            if(kill is null) throw new NotFoundException($"Kill with id {id} not found");

            var playerOwnsResourceResult =
                authorizationService.AuthorizeAsync(user, kill.PlayerId,
                    new PlayerOwnsResourceRequirement()).Result;
            
            var playerIsInTheSameRoomAsResourceResult =
                authorizationService.AuthorizeAsync(user, kill.RoomId,
                    new PlayerIsInTheSameRoomAsResourceRequirement()).Result;

            if (!playerOwnsResourceResult.Succeeded || !playerIsInTheSameRoomAsResourceResult.Succeeded) throw new ForbidException($"You're unauthorize to manipulate this resource");

            dbContext.Kill.Remove(kill);
            dbContext.SaveChanges();
        }
    }
}
