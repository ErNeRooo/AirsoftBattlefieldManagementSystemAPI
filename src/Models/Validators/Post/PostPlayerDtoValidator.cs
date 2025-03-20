using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using FluentValidation;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Validators
{
    public class PostPlayerDtoValidator : AbstractValidator<PostPlayerDto>
    {
        public PostPlayerDtoValidator()
        {
            RuleFor(p => p.Name).NotEmpty().MaximumLength(20);
        }
    }
}
