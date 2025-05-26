using System.Security.Claims;
using AirsoftBattlefieldManagementSystemAPI.Authorization;
using AirsoftBattlefieldManagementSystemAPI.Exceptions;
using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Battle;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace AirsoftBattlefieldManagementSystemAPI.Services.BattleService
{
    public class BattleService(IMapper mapper, IBattleManagementSystemDbContext dbContext, IAuthorizationService authorizationService) : IBattleService
    {
        public BattleDto? GetById(int id, ClaimsPrincipal user)
        {
            Battle? battle = dbContext.Battle.FirstOrDefault(t => t.BattleId == id);

            if (battle is null) throw new NotFoundException($"Battle with id {id} not found");

            var playerIsInTheSameRoomAsResourceResult =
                authorizationService.AuthorizeAsync(user, battle.RoomId,
                    new PlayerIsInTheSameRoomAsResourceRequirement()).Result;

            if (!playerIsInTheSameRoomAsResourceResult.Succeeded) throw new ForbidException($"You're unauthorize to manipulate this resource");
            
            BattleDto battleDto = mapper.Map<BattleDto>(battle);

            return battleDto;
        }

        public int Create(PostBattleDto battleDto, ClaimsPrincipal user)
        {
            Room room = dbContext.Room.FirstOrDefault(r => r.RoomId == battleDto.RoomId);
            
            var playerOwnsResourceResult =
                authorizationService.AuthorizeAsync(user, room.AdminPlayerId,
                    new PlayerOwnsResourceRequirement()).Result;
            
            var playerIsInTheSameRoomAsResourceResult =
                authorizationService.AuthorizeAsync(user, battleDto.RoomId,
                    new PlayerIsInTheSameRoomAsResourceRequirement()).Result;

            if (!playerOwnsResourceResult.Succeeded || !playerIsInTheSameRoomAsResourceResult.Succeeded) throw new ForbidException($"You're unauthorize to manipulate this resource");

            Battle battle = mapper.Map<Battle>(battleDto);
            dbContext.Battle.Add(battle);
            dbContext.SaveChanges();
            return battle.BattleId;
        }

        public void Update(int id, PutBattleDto battleDto, ClaimsPrincipal user)
        {
            Battle? previousBattle = dbContext.Battle.FirstOrDefault(t => t.BattleId == id);

            if (previousBattle is null) throw new NotFoundException($"Battle with id {id} not found");
            
            Room room = dbContext.Room.FirstOrDefault(r => r.RoomId == previousBattle.RoomId);
            
            var playerOwnsResourceResult =
                authorizationService.AuthorizeAsync(user, room.AdminPlayerId,
                    new PlayerOwnsResourceRequirement()).Result;
            
            var playerIsInTheSameRoomAsResourceResult =
                authorizationService.AuthorizeAsync(user, previousBattle.RoomId,
                    new PlayerIsInTheSameRoomAsResourceRequirement()).Result;

            if (!playerOwnsResourceResult.Succeeded || !playerIsInTheSameRoomAsResourceResult.Succeeded) throw new ForbidException($"You're unauthorize to manipulate this resource");

            mapper.Map(battleDto, previousBattle);
            dbContext.Battle.Update(previousBattle);
            dbContext.SaveChanges();
        }

        public void DeleteById(int id, ClaimsPrincipal user)
        {
            Battle? battle = dbContext.Battle.FirstOrDefault(t => t.BattleId == id);

            if (battle is null) throw new NotFoundException($"Battle with id {id} not found");

            Room room = dbContext.Room.FirstOrDefault(r => r.RoomId == battle.RoomId);
            
            var playerOwnsResourceResult =
                authorizationService.AuthorizeAsync(user, room.AdminPlayerId,
                    new PlayerOwnsResourceRequirement()).Result;
            
            var playerIsInTheSameRoomAsResourceResult =
                authorizationService.AuthorizeAsync(user, battle.RoomId,
                    new PlayerIsInTheSameRoomAsResourceRequirement()).Result;

            if (!playerOwnsResourceResult.Succeeded || !playerIsInTheSameRoomAsResourceResult.Succeeded) throw new ForbidException($"You're unauthorize to manipulate this resource");

            dbContext.Battle.Remove(battle);
            dbContext.SaveChanges();
        }
    }
}
