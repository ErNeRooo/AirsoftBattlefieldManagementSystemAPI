﻿using System.ComponentModel.DataAnnotations;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get
{
    public class AccountDto
    {
        public int AccountId { get; set; }
        public string Email { get; set; }
    }
}
