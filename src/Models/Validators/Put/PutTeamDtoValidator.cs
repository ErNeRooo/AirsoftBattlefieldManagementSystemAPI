using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using FluentValidation;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Validators
{
    public class PutTeamDtoValidator : AbstractValidator<PutTeamDto>
    {
        public PutTeamDtoValidator(IBattleManagementSystemDbContext dbContext)
        {
            RuleFor(t => t.Name).NotEmpty().MinimumLength(1).MaximumLength(60);
            RuleFor(t => t.OfficerPlayerId).NotEmpty().Custom((value, context) =>
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
