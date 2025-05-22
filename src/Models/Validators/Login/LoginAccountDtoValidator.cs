using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Login;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using FluentValidation;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Validators.Login
{
    public class LoginAccountDtoValidator : AbstractValidator<LoginAccountDto>
    {
        public LoginAccountDtoValidator(IBattleManagementSystemDbContext dbContext)
        {
            RuleFor(r => r.Email).NotEmpty().EmailAddress().Custom((value, context) =>
            {
                var isNotFound = !dbContext.Account.Any(r => r.Email == value);

                if(isNotFound) context.AddFailure($"Account with email {value} not found");
            });
            RuleFor(a => a.Password).NotEmpty().MinimumLength(10);
        }
    }
}
