using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using FluentValidation;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Validators
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
