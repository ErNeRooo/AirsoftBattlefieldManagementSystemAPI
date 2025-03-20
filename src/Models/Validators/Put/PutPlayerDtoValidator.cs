using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using FluentValidation;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Validators
{
    public class PutPlayerDtoValidator : AbstractValidator<PutPlayerDto>
    {
        public PutPlayerDtoValidator(IBattleManagementSystemDbContext dbContext)
        {
            RuleFor(p => p.Name).NotEmpty().MaximumLength(20);
            RuleFor(p => p.IsDead).NotEmpty();
            RuleFor(p => p.RoomId).NotEmpty().Custom((value, context) =>
            {
                bool isNotExiting = !dbContext.Room.Any(p => p.RoomId == value);

                if (isNotExiting)
                {
                    context.AddFailure("RoomId", $"Room with id {value} not found");
                }
            });
            RuleFor(p => p.TeamId).NotEmpty().Custom((value, context) =>
            {
                bool isNotExiting = !dbContext.Team.Any(p => p.TeamId == value);

                if (isNotExiting)
                {
                    context.AddFailure("TeamId", $"Team with id {value} not found");
                }
            });
            RuleFor(p => p.AccountId).NotEmpty().Custom((value, context) =>
            {
                bool isNotExiting = !dbContext.Account.Any(p => p.AccountId == value);

                if (isNotExiting)
                {
                    context.AddFailure("AccountId", $"Account with id {value} not found");
                }
            });
        }
    }
}
