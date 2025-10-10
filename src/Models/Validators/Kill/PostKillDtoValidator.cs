using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Kill;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Validators.Kill
{
    public class PostKillDtoValidator : AbstractValidator<PostKillDto>
    {
        public PostKillDtoValidator()
        {
            RuleFor(k => k.Latitude)
                .InclusiveBetween(-90, 90);
            
            RuleFor(k => k.Longitude)
                .InclusiveBetween(-180, 180);

            RuleFor(k => k.Accuracy)
                .GreaterThanOrEqualTo(0);
            
            RuleFor(k => k.Bearing)
                .InclusiveBetween((short)0, (short)360);
            
            RuleFor(k => k.Time)
                .NotEmpty();
        }
    }
}
