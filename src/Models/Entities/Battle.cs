﻿namespace AirsoftBattlefieldManagementSystemAPI.Models.Entities
{
    public class Battle
    {
        public int BattleId { get; set; }
        public bool IsActive { get; set; } = false;
        public string Name { get; set; }
        public int? RoomId { get; set; }
        public virtual Room Room { get; set; }
        public virtual List<PlayerLocation> PlayerLocations { get; set; }
        public virtual List<Kill> Kills { get; set; }
        public virtual List<Death> Deaths { get; set; }
    }
}
