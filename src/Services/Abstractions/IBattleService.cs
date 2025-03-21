﻿using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update;

namespace AirsoftBattlefieldManagementSystemAPI.Services.Abstractions
{
    public interface IBattleService
    {
        public BattleDto GetById(int id);
        public int Create(PostBattleDto postBattleDto);
        public void Update(int id, PutBattleDto battleDto);
        public void DeleteById(int id);
    }
}
