using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Account;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Validators.Account
{
    public class PostAccountDtoValidator : AbstractValidator<PostAccountDto>
    {
        public PostAccountDtoValidator(IBattleManagementSystemDbContext dbContext)
        {
            RuleFor(a => a.Email).NotEmpty().EmailAddress().Custom((value, context) =>
            {
                bool isExiting = dbContext.Account.Any(p => p.Email == value);
                
                if (isExiting)
                {
                    context.AddFailure("Email", $"There is already account with email {value}");
                }
            });
            RuleFor(a => a.Password).NotEmpty().MinimumLength(10);
        }
    }
}
