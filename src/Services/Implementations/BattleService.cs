using AirsoftBattlefieldManagementSystemAPI.Exceptions;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Services.Abstractions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AirsoftBattlefieldManagementSystemAPI.Services.Implementations
{
    public class BattleService(IMapper mapper, IBattleManagementSystemDbContext dbContext) : IBattleService
    {
        public BattleDto? GetById(int id)
        {
            Battle? battle = dbContext.Battle.FirstOrDefault(t => t.BattleId == id);

            if (battle is null) throw new NotFoundException($"Battle with id {id} not found");

            BattleDto battleDto = mapper.Map<BattleDto>(battle);

            return battleDto;
        }

        public int Create(PostBattleDto battleDto)
        {
            Battle battle = mapper.Map<Battle>(battleDto);
            dbContext.Battle.Add(battle);
            dbContext.SaveChanges();
            return battle.BattleId;
        }

        public void Update(int id, PutBattleDto battleDto)
        {
            Battle? previousBattle = dbContext.Battle.FirstOrDefault(t => t.BattleId == id);

            if (previousBattle is null) throw new NotFoundException($"Battle with id {id} not found");

            mapper.Map(battleDto, previousBattle);
            dbContext.Battle.Update(previousBattle);
            dbContext.SaveChanges();
        }

        public void DeleteById(int id)
        {
            Battle? battle = dbContext.Battle.FirstOrDefault(t => t.BattleId == id);

            if (battle is null) throw new NotFoundException($"Battle with id {id} not found");

            dbContext.Battle.Remove(battle);
            dbContext.SaveChanges();
        }
    }
}
