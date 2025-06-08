using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Team;
using FluentValidation;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Validators.Team
{
    public class PutTeamDtoValidator : AbstractValidator<PutTeamDto>
    {
        public PutTeamDtoValidator(IBattleManagementSystemDbContext dbContext)
        {
            RuleFor(t => t.Name)
                .Custom((value, context) =>
                {
                    if(string.IsNullOrEmpty(value)) return;

                    if (value.Length < 1 || value.Length > 60) context.AddFailure("Name must be between 1 and 60 characters.");
                });
            
            RuleFor(t => t.OfficerPlayerId)
                .Custom((value, context) =>
                {
                    if(value == null) return;
                    
                    bool isNotExiting = !dbContext.Player.Any(p => p.PlayerId == value);

                    if (isNotExiting)
                    {
                        context.AddFailure("OfficerPlayerId", $"Player with id {value} not found");
                    }
                });
        }
    }
}
