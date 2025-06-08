
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Location;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Validators.Location
{
    public class PutLocationDtoValidator : AbstractValidator<PutLocationDto>
    {
        public PutLocationDtoValidator()
        {
            RuleFor(l => l.Latitude)
                .InclusiveBetween(-90, 90);
            
            RuleFor(l => l.Longitude)
                .InclusiveBetween(-180, 180);

            RuleFor(l => l.Accuracy)
                .GreaterThanOrEqualTo(0);
            
            RuleFor(l => l.Bearing)
                .InclusiveBetween((short)0, (short)360);
        }
    }
}
