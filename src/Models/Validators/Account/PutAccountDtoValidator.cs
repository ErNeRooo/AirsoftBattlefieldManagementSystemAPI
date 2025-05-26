using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Account;
using FluentValidation;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Validators.Account
{
    public class PutAccountDtoValidator : AbstractValidator<PutAccountDto>
    {
        public PutAccountDtoValidator(IBattleManagementSystemDbContext dbContext)
        {
            RuleFor(a => a.Email).EmailAddress().Custom((value, context) =>
            {
                bool isExiting = dbContext.Account.Any(p => p.Email == value);

                if (isExiting)
                {
                    context.AddFailure("Email", $"There is already account with email {value}");
                }
            });
            RuleFor(a => a.Password).MinimumLength(10);
        }
    }
}
