﻿using System.ComponentModel.DataAnnotations;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create
{
    public class CreateTeamDto
    {
        public string Name { get; set; }
        public int RoomId { get; set; }
        public int OfficerPlayerId { get; set; }
    }
}
