using AirsoftBattlefieldManagementSystemAPI.Models.Dtos;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace AirsoftBattlefieldManagementSystemAPI.Services
{
    public class PlayerService(BmsDbContext dbContext, IMapper mapper) : IPlayerService
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
    }
}
