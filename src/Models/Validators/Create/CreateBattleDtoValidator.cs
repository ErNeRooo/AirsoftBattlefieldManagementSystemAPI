using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Validators
{
    public class CreateBattleDtoValidator : AbstractValidator<CreateBattleDto>
    {
        public CreateBattleDtoValidator(IBattleManagementSystemDbContext dbContext)
        {
            RuleFor(p => p.Name).NotEmpty().MaximumLength(60);
            RuleFor(p => p.RoomId).NotEmpty().Custom((value, context) =>
            {
                bool isNotExiting = !dbContext.Room.Any(p => p.RoomId == value);

                if (isNotExiting)
                {
                    context.AddFailure("RoomId", $"Room with id {value} not found");
                }
            }); ;
        }
    }
}
