using AirsoftBattlefieldManagementSystemAPI.Enums;
using AirsoftBattlefieldManagementSystemAPI.Exceptions;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Services.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AirsoftBattlefieldManagementSystemAPI.Services.Implementations
{
    public class RoomService(IMapper mapper, IBattleManagementSystemDbContext dbContext, IPasswordHasher<Room> passwordHasher, IJoinCodeService joinCodeService) : IRoomService
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

        public string Create(PostRoomDto roomDto)
        {
            if (roomDto.JoinCode is null)
            {
                roomDto.JoinCode = joinCodeService.Generate(JoinCodeFormat.From0to9, 6);
            }

            Room room = mapper.Map<Room>(roomDto);

            var hash = passwordHasher.HashPassword(room, room.PasswordHash);
            room.PasswordHash = hash;

            dbContext.Room.Add(room);
            dbContext.SaveChanges();

            return room.JoinCode;
        }

        public void Update(int id, PutRoomDto roomDto)
        {
            Room? previousRoom = dbContext.Room.FirstOrDefault(r => r.RoomId == id);

            if(previousRoom is null) throw new NotFoundException($"Room with id {id} not found");

            Room updatedRoom = mapper.Map(roomDto, previousRoom);

            var hash = passwordHasher.HashPassword(updatedRoom, updatedRoom.PasswordHash);
            updatedRoom.PasswordHash = hash;
            
            dbContext.Room.Update(updatedRoom);
            dbContext.SaveChanges();
        }

        public void DeleteById(int id)
        {
            Room? room = dbContext.Room.FirstOrDefault(r => r.RoomId == id);
            
            if (room is null) throw new NotFoundException($"Room with id {id} not found");

            dbContext.Room.Remove(room);
            dbContext.SaveChanges();
        }
    }
}
