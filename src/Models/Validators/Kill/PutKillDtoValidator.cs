using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Kill;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Location;
using FluentValidation;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Validators.Kill
{
    public class PutKillDtoValidator : AbstractValidator<PutKillDto>
    {
        public PutKillDtoValidator()
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
