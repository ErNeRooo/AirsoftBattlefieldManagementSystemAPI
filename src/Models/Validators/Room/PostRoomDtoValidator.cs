using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Room;
using FluentValidation;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Validators.Room
{
    public class PostRoomDtoValidator : AbstractValidator<PostRoomDto>
    {
        public PostRoomDtoValidator(IBattleManagementSystemDbContext dbContext)
        {
            RuleFor(r => r.MaxPlayers)
                .NotEmpty()
                .GreaterThan(1)
                .LessThanOrEqualTo(100000);

            RuleFor(r => r.JoinCode)
                .Length(6)
                .Custom((value, context) =>
                {
                    bool isJoinCodeOccupied = dbContext.Room.Any(r => r.JoinCode == value);

                    if (isJoinCodeOccupied)
                    {
                        context.AddFailure("JoinCode", $"Join Code {value} is occupied");
                    }
                });
        }
    }
}
