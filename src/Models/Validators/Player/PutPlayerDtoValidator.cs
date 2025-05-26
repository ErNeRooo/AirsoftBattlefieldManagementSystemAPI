using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Player;
using FluentValidation;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Validators.Player
{
    public class PutPlayerDtoValidator : AbstractValidator<PutPlayerDto>
    {
        public PutPlayerDtoValidator(IBattleManagementSystemDbContext dbContext)
        {
            RuleFor(p => p.Name).MaximumLength(20);
            RuleFor(p => p.TeamId).Custom((value, context) =>
            {
                bool isNotExiting = !dbContext.Team.Any(p => p.TeamId == value);

                if (isNotExiting)
                {
                    context.AddFailure("TeamId", $"Team with id {value} not found");
                }
            });
        }
    }
}
