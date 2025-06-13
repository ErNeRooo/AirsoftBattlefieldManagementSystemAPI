using System.Text.RegularExpressions;
using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Room;
using FluentValidation;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Validators.Room
{
    public class PutRoomDtoValidator : AbstractValidator<PutRoomDto>
    {
        public PutRoomDtoValidator(IBattleManagementSystemDbContext dbContext)
        {
            RuleFor(r => r.MaxPlayers)
                .Custom((value, context) =>
                {
                    if(value is null) return;

                    if (value < 2 || value > 100000) context.AddFailure("MaxPlayers must be between 1 and 100000");
                });

            RuleFor(r => r.JoinCode)
                .Custom((value, context) =>
                {
                    if(string.IsNullOrEmpty(value)) return;
                    
                    if(value.Length != 6) context.AddFailure("Room join code must be exactly 6 characters long");
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

            RuleFor(r => r.AdminPlayerId)
                .Custom((value, context) =>
                {
                    if(value is null) return;
                    
                    bool isExisting = dbContext.Player.Any(r => r.PlayerId == value);

                    if (!isExisting)
                    {
                        context.AddFailure("AdminPlayerId", $"There is no player with id {value}");
                    }
                });
        }
    }
}
