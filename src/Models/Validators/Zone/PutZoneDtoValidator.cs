using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Zone;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using FluentValidation;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Validators.Zone;

public class PutZoneDtoValidator : AbstractValidator<PutZoneDto>
{
    public PutZoneDtoValidator()
    {
        RuleFor(zone => zone.Name)
            .Custom((value, context) =>
            {
                if(string.IsNullOrEmpty(value)) return;
                
                if(value.Length >= 50) context.AddFailure("zone name must be less than 50 characters");
            });
        
        RuleFor(k => k.Type)
            .Custom((value, context) =>
            {
                if(string.IsNullOrEmpty(value)) return;
                
                List<string> types = typeof(ZoneTypes)
                    .GetFields()
                    .Where(f => f.FieldType == typeof(string))
                    .Select(f => (string)f.GetRawConstantValue()!)
                    .ToList();

                if(!types.Contains(value)) context.AddFailure("Invalid type of map zone");
            });
    }
}
