﻿using System.ComponentModel.DataAnnotations;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Battle
{
    public class PostBattleDto
    {
        public string Name { get; set; }
        public bool IsActive { get; set; } = false;
        public int RoomId { get; set; }
    }
}
