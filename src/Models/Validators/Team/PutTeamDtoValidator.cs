using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Team;
using FluentValidation;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Validators.Team
{
    public class PutTeamDtoValidator : AbstractValidator<PutTeamDto>
    {
        public PutTeamDtoValidator(IBattleManagementSystemDbContext dbContext)
        {
            RuleFor(t => t.Name).MinimumLength(1).MaximumLength(60);
            RuleFor(t => t.OfficerPlayerId).Custom((value, context) =>
            {
                bool isNotExiting = !dbContext.Player.Any(p => p.PlayerId == value);

                if (isNotExiting)
                {
                    context.AddFailure("OfficerPlayerId", $"Player with id {value} not found");
                }
            });
        }
    }
}
