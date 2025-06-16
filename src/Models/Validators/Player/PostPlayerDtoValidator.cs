using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Player;
using FluentValidation;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Validators.Player
{
    public class PostPlayerDtoValidator : AbstractValidator<PostPlayerDto>
    {
        public PostPlayerDtoValidator()
        {
            RuleFor(p => p.Name).NotEmpty().MaximumLength(40);
        }
    }
}
