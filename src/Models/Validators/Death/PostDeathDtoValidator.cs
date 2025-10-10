
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Death;
using FluentValidation;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Validators.Death
{
    public class PostDeathDtoValidator : AbstractValidator<PostDeathDto>
    {
        public PostDeathDtoValidator()
        {
            RuleFor(l => l.Latitude)
                .InclusiveBetween(-90, 90);
            
            RuleFor(l => l.Longitude)
                .InclusiveBetween(-180, 180);

            RuleFor(l => l.Accuracy)
                .GreaterThanOrEqualTo(0);
            
            RuleFor(l => l.Bearing)
                .InclusiveBetween((short)0, (short)360);
            
            RuleFor(l => l.Time)
                .NotEmpty();
        }
    }
}
