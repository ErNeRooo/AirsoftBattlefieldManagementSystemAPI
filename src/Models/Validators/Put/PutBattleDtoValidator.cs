﻿using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using FluentValidation;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Validators
{
    public class PutBattleDtoValidator : AbstractValidator<PutBattleDto>
    {
        public PutBattleDtoValidator()
        {
            RuleFor(p => p.Name).MaximumLength(60);
        }
    }
}
