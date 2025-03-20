using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using FluentValidation;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Validators
{
    public class PostRoomDtoValidator : AbstractValidator<PostRoomDto>
    {
        public PostRoomDtoValidator(IBattleManagementSystemDbContext dbContext)
        {
            RuleFor(r => r.AdminPlayerId).NotEmpty();
            RuleFor(r => r.MaxPlayers).NotEmpty().GreaterThan(1).LessThan(100000);

            RuleFor(r => r.JoinCode).Length(6).Custom((value, context) =>
            {
                bool isJoinCodeOccupied = dbContext.Room.Any(r => r.JoinCode == value);

                if (isJoinCodeOccupied)
                {
                    context.AddFailure("JoinCode", $"Join Code {value} is occupied");
                }
            });
            RuleFor(r => r.AdminPlayerId).Custom((value, context) =>
            {
                bool isExisting = dbContext.Room.Any(r => r.AdminPlayerId == value);

                if (!isExisting)
                {
                    context.AddFailure("AdminPlayerId", $"There is not a player with id {value}");
                }
            });
        }
    }
}
