﻿using System.Security.Claims;
using AirsoftBattlefieldManagementSystemAPI.Enums;
using AirsoftBattlefieldManagementSystemAPI.Exceptions;
using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Room;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Services.AuthorizationHelperService;
using AirsoftBattlefieldManagementSystemAPI.Services.ClaimsHelperService;
using AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService;
using AirsoftBattlefieldManagementSystemAPI.Services.JoinCodeService;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace AirsoftBattlefieldManagementSystemAPI.Services.RoomService
{
    public class RoomService(IMapper mapper, IBattleManagementSystemDbContext dbContext, IDbContextHelperService dbHelper, IPasswordHasher<Room> passwordHasher, IJoinCodeService joinCodeService, IAuthorizationHelperService authorizationHelperService, IClaimsHelperService claimsHelper) : IRoomService
    {
        public RoomDto GetById(int id)
        {
            Room room = dbHelper.Room.FindById(id);

            RoomDto roomDto = mapper.Map<RoomDto>(room);
            return roomDto;
        }

        public RoomDto GetByJoinCode(string joinCode)
        {
            if(joinCode.Length != 6) throw new InvalidJoinCodeFormatException("Invalid join code");
            
            Room? room = dbHelper.Room.FindByJoinCode(joinCode);

            RoomDto roomDto = mapper.Map<RoomDto>(room);
            return roomDto;
        }

        public RoomDto Create(PostRoomDto roomDto, ClaimsPrincipal user)
        {
            int playerId = claimsHelper.GetIntegerClaimValue("playerId", user);
            
            Player player = dbHelper.Player.FindById(playerId);
            
            if(player.RoomId is not null && player.RoomId != 0) throw new PlayerAlreadyInsideRoomException("You are already inside a room");
            
            if (roomDto.JoinCode is null)
            {
                roomDto.JoinCode = joinCodeService.Generate(JoinCodeFormat.From0to9, 6);
            }

            Room room = mapper.Map<Room>(roomDto);
            
            room.AdminPlayerId = claimsHelper.GetIntegerClaimValue("playerId", user);

            var hash = passwordHasher.HashPassword(room, room.PasswordHash);
            room.PasswordHash = hash;

            dbContext.Room.Add(room);
            dbContext.SaveChanges();

            return mapper.Map<RoomDto>(room);
        }

        public RoomDto Update(PutRoomDto roomDto, ClaimsPrincipal user)
        {
            int playerId = claimsHelper.GetIntegerClaimValue("playerId", user);
            Player player = dbHelper.Player.FindById(playerId);
            Room previousRoom = dbHelper.Room.FindById(player.RoomId);

            if(previousRoom.AdminPlayerId is not null) authorizationHelperService.CheckPlayerOwnsResource(user, previousRoom.AdminPlayerId);
            
            Room updatedRoom = mapper.Map(roomDto, previousRoom);

            var hash = passwordHasher.HashPassword(updatedRoom, updatedRoom.PasswordHash);
            updatedRoom.PasswordHash = hash;
            
            dbContext.Room.Update(updatedRoom);
            dbContext.SaveChanges();
            
            return mapper.Map<RoomDto>(updatedRoom);
        }

        public void Delete(ClaimsPrincipal user)
        {
            int playerId = claimsHelper.GetIntegerClaimValue("playerId", user);
            Player player = dbHelper.Player.FindById(playerId);
            Room room = dbHelper.Room.FindById(player.RoomId);

            authorizationHelperService.CheckPlayerOwnsResource(user, room.AdminPlayerId);

            dbContext.Room.Remove(room);
            dbContext.SaveChanges();
        }

        public RoomDto Join(LoginRoomDto roomDto, ClaimsPrincipal user)
        {
            string joinCode = roomDto.JoinCode;
            string password = roomDto.Password;

            int playerId = claimsHelper.GetIntegerClaimValue("playerId", user);

            Room room = dbHelper.Room.FindByJoinCode(joinCode);
            Player player = dbHelper.Player.FindById(playerId);

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
            int playerId = claimsHelper.GetIntegerClaimValue("playerId", user);

            Player player = dbHelper.Player.FindById(playerId);

            if(player.RoomId is not null || player.RoomId == 0)
            {
                Room room = dbHelper.Room.FindById(player.RoomId);
                Team team = dbHelper.Team.FindById(player.TeamId);
                
                if(team.OfficerPlayerId == playerId) team.OfficerPlayerId = null;
                if(room.AdminPlayerId == playerId) room.AdminPlayerId = null;
                
                dbContext.Team.Update(team);
                dbContext.Room.Update(room);
            }

            player.RoomId = null;
            player.TeamId = null;
            dbContext.Player.Update(player);
            
            dbContext.SaveChanges();
        }
    }
}
