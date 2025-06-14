﻿
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Location;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Validators.Location
{
    public class PostLocationDtoValidator : AbstractValidator<PostLocationDto>
    {
        public PostLocationDtoValidator()
        {
            RuleFor(l => l.Latitude)
                .NotEmpty()
                .InclusiveBetween(-90, 90);
            
            RuleFor(l => l.Longitude)
                .NotEmpty()
                .InclusiveBetween(-180, 180);

            RuleFor(l => l.Accuracy)
                .NotEmpty()
                .GreaterThanOrEqualTo(0);
            
            RuleFor(l => l.Bearing)
                .NotEmpty()
                .InclusiveBetween((short)0, (short)360);
            
            RuleFor(l => l.Time)
                .NotEmpty();
        }
    }
}
