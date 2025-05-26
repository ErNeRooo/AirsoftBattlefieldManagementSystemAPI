
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Battle;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using FluentValidation;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Validators.Battle
{
    public class PutBattleDtoValidator : AbstractValidator<PutBattleDto>
    {
        public PutBattleDtoValidator()
        {
            RuleFor(p => p.Name).MaximumLength(60);
        }
    }
}
