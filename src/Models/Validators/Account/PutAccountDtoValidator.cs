using System.Text.RegularExpressions;
using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Account;
using FluentValidation;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Validators.Account
{
    public class PutAccountDtoValidator : AbstractValidator<PutAccountDto>
    {
        public PutAccountDtoValidator(IBattleManagementSystemDbContext dbContext)
        {
            RuleFor(a => a.Email)                
                .Custom((value, context) =>
                {
                    if(string.IsNullOrEmpty(value)) return;
                    
                    bool matchRegex = Regex.IsMatch(value, "^((?:[A-Za-z0-9!#$%&'*+\\-\\/=?^_`{|}~]|(?<=^|\\.)\"|\"(?=$|\\.|@)|(?<=\".*)[ .](?=.*\")|(?<!\\.)\\.){1,64})(@)((?:[A-Za-z0-9.\\-])*(?:[A-Za-z0-9])\\.(?:[A-Za-z0-9]){2,})$");
                        
                    if(!matchRegex) context.AddFailure("Email address is not valid");
                })
                .Custom((value, context) =>
                {
                    if(string.IsNullOrEmpty(value)) return;
                    
                    bool isExiting = dbContext.Account.Any(p => p.Email == value);

                    if (isExiting)
                    {
                        context.AddFailure("Email", $"There is already account with email {value}");
                    }
                });
            
            RuleFor(a => a.Password)
                .Custom((value, context) =>
                {
                    if(string.IsNullOrEmpty(value)) return;
                    
                    if(value.Length < 10) context.AddFailure("Password must be at least 10 characters");
                })
                .Custom((value, context) =>
                {
                    if(string.IsNullOrEmpty(value)) return;
                    
                    bool matchRegex = Regex.IsMatch(value, "^(?=.*[A-Z])(?=.*\\d)(?=.*[^a-zA-Z0-9]).+$");
                    
                    if(!matchRegex) context.AddFailure("Password must have at least 1 special character, 1 digit and 1 upper case letter");
                });
        }
    }
}
