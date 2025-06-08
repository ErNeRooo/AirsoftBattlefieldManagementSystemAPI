using System.Text.RegularExpressions;
using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Account;
using FluentValidation;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Validators.Account
{
    public class LoginAccountDtoValidator : AbstractValidator<LoginAccountDto>
    {
        public LoginAccountDtoValidator(IBattleManagementSystemDbContext dbContext)
        {
            RuleFor(r => r.Email)
                .NotEmpty()
                .Custom((value, context) =>
                {
                    bool matchRegex = Regex.IsMatch(value, "^((?:[A-Za-z0-9!#$%&'*+\\-\\/=?^_`{|}~]|(?<=^|\\.)\"|\"(?=$|\\.|@)|(?<=\".*)[ .](?=.*\")|(?<!\\.)\\.){1,64})(@)((?:[A-Za-z0-9.\\-])*(?:[A-Za-z0-9])\\.(?:[A-Za-z0-9]){2,})$");
                    
                    if(!matchRegex) context.AddFailure("Email address is not valid");
                })
                .Custom((value, context) =>
                {
                    var isNotFound = !dbContext.Account.Any(r => r.Email == value);

                    if(isNotFound) context.AddFailure($"Account with email {value} not found");
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
