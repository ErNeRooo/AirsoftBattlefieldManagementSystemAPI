﻿using System.ComponentModel.DataAnnotations;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Account
{
    public class PostAccountDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
