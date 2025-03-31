using System.Text.RegularExpressions;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Validators
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
