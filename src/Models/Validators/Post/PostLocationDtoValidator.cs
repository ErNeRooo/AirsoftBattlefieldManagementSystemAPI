﻿using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Validators
{
    public class PostLocationDtoValidator : AbstractValidator<PostLocationDto>
    {
        public PostLocationDtoValidator()
        {
            RuleFor(l => l.Longitude).NotEmpty();
            RuleFor(l => l.Latitude).NotEmpty();
            RuleFor(l => l.Accuracy).NotEmpty();
            RuleFor(l => l.Bearing).NotEmpty();
            RuleFor(l => l.Time).NotEmpty();
        }
    }
}
