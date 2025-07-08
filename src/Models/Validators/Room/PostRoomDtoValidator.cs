using System.Text.RegularExpressions;
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

            RuleFor(r => r.Password);

            RuleFor(r => r.JoinCode)
                .Custom((value, context) =>
                {
                    if(string.IsNullOrEmpty(value)) return;
                    
                    if(value.Length != 6) context.AddFailure("JoinCode", "If specified, the join code must be 6 characters long.");
                })
                .Custom((value, context) =>
                {
                    if(string.IsNullOrEmpty(value)) return;
                    
                    bool containsNonAlphaNumeric = Regex.IsMatch(value, "[^0-9a-zA-Z]");

                    if (containsNonAlphaNumeric)
                    {
                        context.AddFailure("JoinCode", "Join code must contain only letters and numbers.");
                    }
                })
                .Custom((value, context) =>
                {
                    if(string.IsNullOrEmpty(value)) return;
                    
                    bool isJoinCodeOccupied = dbContext.Room.Any(r => r.JoinCode == value);

                    if (isJoinCodeOccupied)
                    {
                        context.AddFailure("JoinCode", $"Join Code {value} is occupied");
                    }
                });
        }
    }
}
