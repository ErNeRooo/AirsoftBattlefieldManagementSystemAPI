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
            RuleFor(a => a.Email)
                .NotEmpty()
                .Custom((value, context) =>
                {
                    bool matchRegex = Regex.IsMatch(value, "^((?:[A-Za-z0-9!#$%&'*+\\-\\/=?^_`{|}~]|(?<=^|\\.)\"|\"(?=$|\\.|@)|(?<=\".*)[ .](?=.*\")|(?<!\\.)\\.){1,64})(@)((?:[A-Za-z0-9.\\-])*(?:[A-Za-z0-9])\\.(?:[A-Za-z0-9]){2,})$");
                    
                    if(!matchRegex) context.AddFailure("Email address is not valid");
                })
                .Custom((value, context) =>
                {
                    bool isExiting = dbContext.Account.Any(p => p.Email == value);
                    
                    if (isExiting)
                    {
                        context.AddFailure("Email", $"There is already account with email {value}");
                    }
                });
            
            RuleFor(a => a.Password)
                .NotEmpty()
                .MinimumLength(10)
                .Custom((value, context) =>
                {
                    bool matchRegex = Regex.IsMatch(value, "^(?=.*[A-Z])(?=.*\\d)(?=.*[^a-zA-Z0-9]).+$");
                    
                    if(!matchRegex) context.AddFailure("Password must have at least 1 special character, 1 digit and 1 upper case letter");
                });
        }
    }
}
