﻿using System.ComponentModel.DataAnnotations;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get
{
    public class DeathDto
    {
        public int DeathId { get; set; }
        public int LocationId { get; set; }
        public int PlayerId { get; set; }
        public int RoomId { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public short Accuracy { get; set; }
        public short Bearing { get; set; }
        public DateTime Time { get; set; }
    }
}
