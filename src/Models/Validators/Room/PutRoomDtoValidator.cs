using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Room;
using FluentValidation;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Validators.Room
{
    public class PutRoomDtoValidator : AbstractValidator<PutRoomDto>
    {
        public PutRoomDtoValidator(IBattleManagementSystemDbContext dbContext)
        {
            RuleFor(r => r.MaxPlayers).GreaterThan(1).LessThan(100000);

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
                bool isExisting = dbContext.Player.Any(r => r.PlayerId == value);

                if (!isExisting)
                {
                    context.AddFailure("AdminPlayerId", $"There is no player with id {value}");
                }
            });
        }
    }
}
