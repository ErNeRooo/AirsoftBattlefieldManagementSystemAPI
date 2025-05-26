
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Death;
using FluentValidation;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Validators.Death
{
    public class PostDeathDtoValidator : AbstractValidator<PostDeathDto>
    {
        public PostDeathDtoValidator()
        {
            RuleFor(l => l.Longitude).NotEmpty();
            RuleFor(l => l.Latitude).NotEmpty();
            RuleFor(l => l.Accuracy).NotEmpty();
            RuleFor(l => l.Bearing).NotEmpty();
            RuleFor(l => l.Time).NotEmpty();
        }
    }
}
