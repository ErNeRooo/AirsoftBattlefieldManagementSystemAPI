using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Login;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using FluentValidation;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Validators.Login
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
