﻿using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Services.Abstractions;
using AutoMapper;

namespace AirsoftBattlefieldManagementSystemAPI.Services.Implementations
{
    public class RoomService(IMapper mapper, IBattleManagementSystemDbContext dbContext) : IRoomService
    {
        public RoomDto? GetById(int id)
        {
            Room? room = dbContext.Room.FirstOrDefault(r => r.RoomId == id);
            if (room is null) return null;

            RoomDto roomDto = mapper.Map<RoomDto>(room);
            return roomDto;
        }

        public int Create(CreateRoomDto roomDto)
        {
            Room room = mapper.Map<Room>(roomDto);

            AvailableId id = dbContext.AvailableId.First();
            room.RoomId = id.Id;
            dbContext.AvailableId.Remove(id);

            dbContext.Room.Add(room);
            dbContext.SaveChanges();

            return room.RoomId;
        }

        public bool Update(int id, UpdateRoomDto playerDto)
        {
            Room? previousRoom = dbContext.Room.FirstOrDefault(r => r.RoomId == id);

            if(previousRoom is null) return false;

            Room updatedRoom = mapper.Map(playerDto, previousRoom);
            dbContext.Room.Update(updatedRoom);
            dbContext.SaveChanges();

            return true;
        }

        public bool DeleteById(int id)
        {
            Room? room = dbContext.Room.FirstOrDefault(r => r.RoomId == id);

            if (room is null) return false;

            var availableId = new AvailableId();
            availableId.Id = id;

            dbContext.AvailableId.Add(availableId);
            dbContext.Room.Remove(room);
            dbContext.SaveChanges();

            return true;
        }
    }
}
