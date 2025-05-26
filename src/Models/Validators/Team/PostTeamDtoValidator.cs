using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Team;
using FluentValidation;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Validators.Team
{
    public class PostTeamDtoValidator : AbstractValidator<PostTeamDto>
    {
        public PostTeamDtoValidator(IBattleManagementSystemDbContext dbContext)
        {
            RuleFor(t => t.Name).NotEmpty().MinimumLength(1).MaximumLength(60);
            RuleFor(t => t.RoomId).NotEmpty().Custom((value, context) =>
            {
                bool isNotExiting = !dbContext.Room.Any(p => p.RoomId == value);

                if (isNotExiting)
                {
                    context.AddFailure("RoomId", $"Room with id {value} not found");
                }
            });
        }
    }
}
