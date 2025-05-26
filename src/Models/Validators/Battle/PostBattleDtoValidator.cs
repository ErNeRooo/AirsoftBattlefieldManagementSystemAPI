using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Battle;
using FluentValidation;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Validators.Battle
{
    public class PostBattleDtoValidator : AbstractValidator<PostBattleDto>
    {
        public PostBattleDtoValidator(IBattleManagementSystemDbContext dbContext)
        {
            RuleFor(p => p.Name).NotEmpty().MaximumLength(60);
            RuleFor(p => p.RoomId).NotEmpty().Custom((value, context) =>
            {
                bool isNotExiting = !dbContext.Room.Any(p => p.RoomId == value);

                if (isNotExiting)
                {
                    context.AddFailure("RoomId", $"Room with id {value} not found");
                }
            }); ;
        }
    }
}
