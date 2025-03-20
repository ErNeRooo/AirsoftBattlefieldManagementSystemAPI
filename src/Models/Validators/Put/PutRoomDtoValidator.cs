using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using FluentValidation;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Validators
{
    public class PutRoomDtoValidator : AbstractValidator<PutRoomDto>
    {
        public PutRoomDtoValidator(IBattleManagementSystemDbContext dbContext)
        {
            RuleFor(r => r.Password).NotEmpty();
            RuleFor(r => r.MaxPlayers).NotEmpty().GreaterThan(1).LessThan(100000);

            RuleFor(r => r.JoinCode).NotEmpty().Length(6).Custom((value, context) =>
            {
                bool isJoinCodeOccupied = dbContext.Room.Any(r => r.JoinCode == value);

                if (isJoinCodeOccupied)
                {
                    context.AddFailure("JoinCode", $"Join Code {value} is occupied");
                }
            });

            RuleFor(r => r.AdminPlayerId).NotEmpty().Custom((value, context) =>
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
