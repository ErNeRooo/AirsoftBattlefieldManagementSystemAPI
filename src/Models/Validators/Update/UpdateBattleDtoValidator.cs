﻿using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using FluentValidation;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Validators
{
    public class UpdateBattleDtoValidator : AbstractValidator<UpdateBattleDto>
    {
        public UpdateBattleDtoValidator(IBattleManagementSystemDbContext dbContext)
        {
            RuleFor(p => p.Name).NotEmpty().MaximumLength(20);
            RuleFor(p => p.IsActive).NotEmpty();
        }
    }
}
