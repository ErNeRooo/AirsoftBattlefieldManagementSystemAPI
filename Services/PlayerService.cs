using AirsoftBattlefieldManagementSystemAPI.Models;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;

namespace AirsoftBattlefieldManagementSystemAPI.Services
{
    public class PlayerService(IBattleManagementSystemDbContext dbContext, IMapper mapper) : IPlayerService
    {
        public PlayerDto? GetById(int id)
        {
            Player? player = dbContext.Players.FirstOrDefault(p => p.PlayerId == id);

            if(player is null) return null;

            PlayerDto playerDto = mapper.Map<PlayerDto>(player);

            return playerDto;
        }

        public int Create(CreatePlayerDto playerDto)
        {
            var player = mapper.Map<Player>(playerDto);

            dbContext.Players.Add(player);
            dbContext.SaveChanges();

            return player.PlayerId;
        }

        public bool Update(int id, UpdatePlayerDto playerDto)
        {
            Player? previousPlayer = dbContext.Players.FirstOrDefault(p => p.PlayerId == id);

            if(previousPlayer is null) return false;

            mapper.Map(playerDto, previousPlayer);
            dbContext.Players.Update(previousPlayer);
            dbContext.SaveChanges();

            return true;
        }

        public bool DeleteById(int id)
        {
            var player = dbContext.Players.FirstOrDefault(p => p.PlayerId == id);

            if(player is null) return false;

            dbContext.Players.Remove(player);
            dbContext.SaveChanges();

            return true;
        }
    }
}
