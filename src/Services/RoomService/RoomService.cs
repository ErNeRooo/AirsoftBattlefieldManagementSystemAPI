using System.Security.Claims;
using AirsoftBattlefieldManagementSystemAPI.Authorization;
using AirsoftBattlefieldManagementSystemAPI.Enums;
using AirsoftBattlefieldManagementSystemAPI.Exceptions;
using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Room;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Services.JoinCodeService;
using AirsoftBattlefieldManagementSystemAPI.Services.RoomService;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AirsoftBattlefieldManagementSystemAPI.Services.Implementations
{
    public class RoomService(IMapper mapper, IBattleManagementSystemDbContext dbContext, IPasswordHasher<Room> passwordHasher, IJoinCodeService joinCodeService, IAuthorizationService authorizationService) : IRoomService
    {
        public RoomDto GetById(int id)
        {
            Room? room = dbContext.Room.FirstOrDefault(r => r.RoomId == id);

            if (room is null) throw new NotFoundException($"Room with id {id} not found");

            RoomDto roomDto = mapper.Map<RoomDto>(room);
            return roomDto;
        }

        public RoomDto GetByJoinCode(string joinCode)
        {
            Room? room = dbContext.Room.FirstOrDefault(r => r.JoinCode == joinCode);

            if (room is null) throw new NotFoundException($"Room with join code {joinCode} not found");

            RoomDto roomDto = mapper.Map<RoomDto>(room);
            return roomDto;
        }

        public int Create(PostRoomDto roomDto, ClaimsPrincipal user)
        {
            if (roomDto.JoinCode is null)
            {
                roomDto.JoinCode = joinCodeService.Generate(JoinCodeFormat.From0to9, 6);
            }

            Room room = mapper.Map<Room>(roomDto);

            var playerIdClaim = user.Claims.FirstOrDefault(c => c.Type == "playerId")?.Value;
            bool isSuccessfull = int.TryParse(playerIdClaim, out int playerId);
            if (!isSuccessfull) throw new ForbidException("Invalid claim playerId");
            room.AdminPlayerId = playerId;

            var hash = passwordHasher.HashPassword(room, room.PasswordHash);
            room.PasswordHash = hash;

            dbContext.Room.Add(room);
            dbContext.SaveChanges();

            return room.RoomId;
        }

        public void Update(int id, PutRoomDto roomDto, ClaimsPrincipal user)
        {
            Room? previousRoom = dbContext.Room.FirstOrDefault(r => r.RoomId == id);

            if(previousRoom is null) throw new NotFoundException($"Room with id {id} not found");

            var playerOwnsResourceResult =
                authorizationService.AuthorizeAsync(user, previousRoom.AdminPlayerId,
                    new PlayerOwnsResourceRequirement()).Result;
            
            var playerIsInTheSameRoomAsResourceResult =
                authorizationService.AuthorizeAsync(user, previousRoom.RoomId,
                    new PlayerIsInTheSameRoomAsResourceRequirement()).Result;

            if (!playerOwnsResourceResult.Succeeded || !playerIsInTheSameRoomAsResourceResult.Succeeded) throw new ForbidException($"You're unauthorize to manipulate this resource");

            Room updatedRoom = mapper.Map(roomDto, previousRoom);

            var hash = passwordHasher.HashPassword(updatedRoom, updatedRoom.PasswordHash);
            updatedRoom.PasswordHash = hash;
            
            dbContext.Room.Update(updatedRoom);
            dbContext.SaveChanges();
        }

        public void DeleteById(int id, ClaimsPrincipal user)
        {
            Room? room = dbContext.Room.FirstOrDefault(r => r.RoomId == id);
            
            if (room is null) throw new NotFoundException($"Room with id {id} not found");

            var playerOwnsResourceResult =
                authorizationService.AuthorizeAsync(user, room.AdminPlayerId,
                    new PlayerOwnsResourceRequirement()).Result;
            
            var playerIsInTheSameRoomAsResourceResult =
                authorizationService.AuthorizeAsync(user, room.RoomId,
                    new PlayerIsInTheSameRoomAsResourceRequirement()).Result;

            if (!playerOwnsResourceResult.Succeeded || !playerIsInTheSameRoomAsResourceResult.Succeeded) throw new ForbidException($"You're unauthorize to manipulate this resource");

            dbContext.Room.Remove(room);
            dbContext.SaveChanges();
        }

        public int Join(LoginRoomDto roomDto, ClaimsPrincipal user)
        {
            string joinCode = roomDto.JoinCode;
            string password = roomDto.Password;

            string? claimPlayerId = user.Claims.FirstOrDefault(c => c.Type == "playerId").Value;
            bool isParsingSuccessfull = int.TryParse(claimPlayerId, out int playerId);

            if (!isParsingSuccessfull) throw new ForbidException("Invalid claim playerId");

            Room room = dbContext.Room.FirstOrDefault(r => r.JoinCode == joinCode);
            Player? player = dbContext.Player.FirstOrDefault(p => p.PlayerId == playerId);

            if (player is null) throw new NotFoundException($"Player with id {playerId} not found");

            var verificationResult = passwordHasher.VerifyHashedPassword(room, room.PasswordHash, password);

            if (verificationResult == PasswordVerificationResult.Success || verificationResult == PasswordVerificationResult.SuccessRehashNeeded || room.AdminPlayerId == player.PlayerId)
            {
                player.RoomId = room.RoomId;
                dbContext.Player.Update(player);
                dbContext.SaveChanges();

                return room.RoomId;
            }
            
            throw new WrongPasswordException("Wrong room password");
        }

        public void Leave(ClaimsPrincipal user)
        {
            string? claimPlayerId = user.Claims.FirstOrDefault(c => c.Type == "playerId").Value;
            bool isSuccessfull = int.TryParse(claimPlayerId, out int playerId);

            if (!isSuccessfull) throw new ForbidException("Invalid claim playerId");

            Player player = dbContext.Player.FirstOrDefault(p => p.PlayerId == playerId);

            player.RoomId = null;

            dbContext.Player.Update(player);
            dbContext.SaveChanges();
        }
    }
}
