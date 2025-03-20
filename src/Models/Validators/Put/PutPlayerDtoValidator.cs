using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using FluentValidation;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Validators
{
    public class PutPlayerDtoValidator : AbstractValidator<PutPlayerDto>
    {
        public PutPlayerDtoValidator(IBattleManagementSystemDbContext dbContext)
        {
            RuleFor(p => p.Name).NotEmpty().MaximumLength(20);
            RuleFor(p => p.IsDead).NotEmpty();
            RuleFor(p => p.TeamId).NotEmpty().Custom((value, context) =>
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
