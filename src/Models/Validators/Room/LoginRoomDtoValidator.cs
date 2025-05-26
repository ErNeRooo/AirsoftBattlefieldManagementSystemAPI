using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Room;
using FluentValidation;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Validators.Room
{
    public class LoginRoomDtoValidator : AbstractValidator<LoginRoomDto>
    {
        public LoginRoomDtoValidator(IBattleManagementSystemDbContext dbContext)
        {
            RuleFor(r => r.JoinCode).NotEmpty().Length(6).Custom((value, context) =>
            {
                var isNotFound = !dbContext.Room.Any(r => r.JoinCode == value);

                if(isNotFound) context.AddFailure($"Room with join code {value} not found");
            });
        }
    }
}
