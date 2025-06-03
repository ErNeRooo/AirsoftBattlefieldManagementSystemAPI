using System.Security.Claims;
using AirsoftBattlefieldManagementSystemAPI.Authorization;
using AirsoftBattlefieldManagementSystemAPI.Enums;
using AirsoftBattlefieldManagementSystemAPI.Exceptions;
using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Room;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Services.AuthorizationHelperService;
using AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService;
using AirsoftBattlefieldManagementSystemAPI.Services.JoinCodeService;
using AirsoftBattlefieldManagementSystemAPI.Services.RoomService;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AirsoftBattlefieldManagementSystemAPI.Services.Implementations
{
    public class RoomService(IMapper mapper, IBattleManagementSystemDbContext dbContext, IDbContextHelperService dbHelper, IPasswordHasher<Room> passwordHasher, IJoinCodeService joinCodeService, IAuthorizationHelperService authorizationHelperService) : IRoomService
    {
        public RoomDto GetById(int id)
        {
            Room room = dbHelper.FindRoomById(id);

            RoomDto roomDto = mapper.Map<RoomDto>(room);
            return roomDto;
        }

        public RoomDto GetByJoinCode(string joinCode)
        {
            Room? room = dbHelper.FindRoomByIJoinCode(joinCode);

            RoomDto roomDto = mapper.Map<RoomDto>(room);
            return roomDto;
        }

        public RoomDto Create(PostRoomDto roomDto, ClaimsPrincipal user)
        {
            if (roomDto.JoinCode is null)
            {
                roomDto.JoinCode = joinCodeService.Generate(JoinCodeFormat.From0to9, 6);
            }

            Room room = mapper.Map<Room>(roomDto);
            
            room.AdminPlayerId = GetPlayerIdFromClaims(user);

            var hash = passwordHasher.HashPassword(room, room.PasswordHash);
            room.PasswordHash = hash;

            dbContext.Room.Add(room);
            dbContext.SaveChanges();

            return mapper.Map<RoomDto>(room);
        }

        public RoomDto Update(int id, PutRoomDto roomDto, ClaimsPrincipal user)
        {
            Room previousRoom = dbHelper.FindRoomById(id);

            authorizationHelperService.CheckPlayerOwnsResource(user, previousRoom.AdminPlayerId);
            
            Room updatedRoom = mapper.Map(roomDto, previousRoom);

            var hash = passwordHasher.HashPassword(updatedRoom, updatedRoom.PasswordHash);
            updatedRoom.PasswordHash = hash;
            
            dbContext.Room.Update(updatedRoom);
            dbContext.SaveChanges();
            
            return mapper.Map<RoomDto>(updatedRoom);
        }

        public void DeleteById(int id, ClaimsPrincipal user)
        {
            Room room = dbHelper.FindRoomById(id);

            authorizationHelperService.CheckPlayerOwnsResource(user, room.AdminPlayerId);

            dbContext.Room.Remove(room);
            dbContext.SaveChanges();
        }

        public RoomDto Join(LoginRoomDto roomDto, ClaimsPrincipal user)
        {
            string joinCode = roomDto.JoinCode;
            string password = roomDto.Password;

            int playerId = GetPlayerIdFromClaims(user);

            Room room = dbContext.Room.FirstOrDefault(r => r.JoinCode == joinCode);
            Player player = dbHelper.FindPlayerById(playerId);

            var verificationResult = passwordHasher.VerifyHashedPassword(room, room.PasswordHash, password);

            if (verificationResult == PasswordVerificationResult.Success || verificationResult == PasswordVerificationResult.SuccessRehashNeeded || room.AdminPlayerId == player.PlayerId)
            {
                player.RoomId = room.RoomId;
                dbContext.Player.Update(player);
                dbContext.SaveChanges();

                return mapper.Map<RoomDto>(room);
            }
            
            throw new WrongPasswordException("Wrong room password");
        }

        public void Leave(ClaimsPrincipal user)
        {
            int playerId = GetPlayerIdFromClaims(user);

            Player player = dbHelper.FindPlayerById(playerId);

            player.RoomId = null;

            dbContext.Player.Update(player);
            dbContext.SaveChanges();
        }

        private int GetPlayerIdFromClaims(ClaimsPrincipal user)
        {
            var playerIdClaim = user.Claims.FirstOrDefault(c => c.Type == "playerId")?.Value;
            
            bool isParsingSuccessfull = int.TryParse(playerIdClaim, out int playerId);
            
            if (!isParsingSuccessfull) throw new ForbidException("Invalid claim playerId");
            
            return playerId;
        }
    }
}
