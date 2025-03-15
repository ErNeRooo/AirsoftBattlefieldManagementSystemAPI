using AirsoftBattlefieldManagementSystemAPI.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Services.Abstractions;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update;
using AirsoftBattlefieldManagementSystemAPI.Exceptions;

namespace AirsoftBattlefieldManagementSystemAPI.Services.Implementations
{
    public class PlayerService(IBattleManagementSystemDbContext dbContext, IMapper mapper) : IPlayerService
    {
        public PlayerDto? GetById(int id)
        {
            Player? player = dbContext.Player.FirstOrDefault(p => p.PlayerId == id);

            if (player is null) throw new NotFoundException($"Player with id {id} not found");

            PlayerDto playerDto = mapper.Map<PlayerDto>(player);

            return playerDto;
        }

        public int Create(CreatePlayerDto playerDto)
        {
            var player = mapper.Map<Player>(playerDto);

            dbContext.Player.Add(player);
            dbContext.SaveChanges();

            return player.PlayerId;
        }

        public void Update(int id, UpdatePlayerDto playerDto)
        {
            Player? previousPlayer = dbContext.Player.FirstOrDefault(p => p.PlayerId == id);

            if (previousPlayer is null) throw new NotFoundException($"Player with id {id} not found");

            mapper.Map(playerDto, previousPlayer);
            dbContext.Player.Update(previousPlayer);
            dbContext.SaveChanges();
        }

        public void DeleteById(int id)
        {
            var player = dbContext.Player.FirstOrDefault(p => p.PlayerId == id);

            if (player is null) throw new NotFoundException($"Player with id {id} not found");

            dbContext.Player.Remove(player);
            dbContext.SaveChanges();
        }
    }
}
