﻿using AirsoftBattlefieldManagementSystemAPI.Exceptions;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Services.Abstractions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AirsoftBattlefieldManagementSystemAPI.Services.Implementations
{
    public class RoomService(IMapper mapper, IBattleManagementSystemDbContext dbContext) : IRoomService
    {
        public RoomDto GetById(int id)
        {
            Room? room = dbContext.Room.FirstOrDefault(r => r.RoomId == id);

            if (room is null) throw new NotFoundException($"Room with id {id} not found");

            RoomDto roomDto = mapper.Map<RoomDto>(room);
            return roomDto;
        }

        public RoomDto GetByRoomJoinNumber(int roomJoinNumber)
        {
            Room? room = dbContext.Room.FirstOrDefault(r => r.JoinRoomNumber == roomJoinNumber);

            if (room is null) throw new NotFoundException($"Room with room join number {roomJoinNumber} not found");

            RoomDto roomDto = mapper.Map<RoomDto>(room);
            return roomDto;
        }

        public int Create(CreateRoomDto roomDto)
        {
            Room room = mapper.Map<Room>(roomDto);

            dbContext.Room.Add(room);

            dbContext.SaveChanges();

            return room.RoomId;
        }

        public void Update(int id, UpdateRoomDto playerDto)
        {
            Room? previousRoom = dbContext.Room.FirstOrDefault(r => r.RoomId == id);

            if(previousRoom is null) throw new NotFoundException($"Room with id {id} not found");

            Room updatedRoom = mapper.Map(playerDto, previousRoom);
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
