using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Validators
{
    public class CreateAccountDtoValidator : AbstractValidator<CreateAccountDto>
    {
        public CreateAccountDtoValidator(IBattleManagementSystemDbContext dbContext)
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
