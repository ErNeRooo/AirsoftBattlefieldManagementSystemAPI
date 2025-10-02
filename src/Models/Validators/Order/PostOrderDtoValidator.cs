using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Kill;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Order;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using FluentValidation;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Validators.Order;

public class PostOrderDtoValidator : AbstractValidator<PostOrderDto>
{
    public PostOrderDtoValidator()
    {
        RuleFor(k => k.Latitude)
            .InclusiveBetween(-90, 90);
            
        RuleFor(k => k.Longitude)
            .InclusiveBetween(-180, 180);

        RuleFor(k => k.Accuracy)
            .GreaterThanOrEqualTo(0);
            
        RuleFor(k => k.Bearing)
            .InclusiveBetween((short)0, (short)360);
            
        RuleFor(k => k.Time)
            .NotEmpty();
        
        RuleFor(k => k.Type)
            .NotEmpty()
            .Custom((value, context) =>
            {
                List<string> types = typeof(OrderTypes)
                    .GetFields()
                    .Where(f => f.FieldType == typeof(string))
                    .Select(f => (string)f.GetRawConstantValue()!)
                    .ToList();

                if(!types.Contains(value)) context.AddFailure("Invalid type");
            });
    }
}