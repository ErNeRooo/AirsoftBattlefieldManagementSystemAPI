﻿namespace AirsoftBattlefieldManagementSystemAPI.Models.Entities
{
    public class Room
    {
        public int RoomId { get; set; }
        public int MaxPlayers { get; set; }
        public string JoinCode { get; set; }
        public string PasswordHash { get; set; }
        
        public virtual Battle Battle { get; set; }
        public int? AdminPlayerId { get; set; }
        public virtual Player AdminPlayer { get; set; }
        public virtual List<Team> Teams { get; set; }
        public virtual List<Player> Players { get; set; }
    }
}
