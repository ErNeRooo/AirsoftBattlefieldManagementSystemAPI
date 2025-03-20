using System.Text.RegularExpressions;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Validators
{
    public class PutLocationDtoValidator : AbstractValidator<PostLocationDto>
    {
        public PutLocationDtoValidator()
        {
            RuleFor(l => l.Longitude).NotEmpty();
            RuleFor(l => l.Latitude).NotEmpty();
            RuleFor(l => l.Accuracy).NotEmpty();
            RuleFor(l => l.Bearing).NotEmpty();
            RuleFor(l => l.Time).NotEmpty();
        }
    }
}
