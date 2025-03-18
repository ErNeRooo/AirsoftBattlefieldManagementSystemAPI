﻿using System.ComponentModel.DataAnnotations;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update
{
    public class UpdateRoomDto
    {
        public int MaxPlayers { get; set; }
        public string JoinCode { get; set; }
        public string Password { get; set; }
        public int AdminPlayerId { get; set; }
    }
}
