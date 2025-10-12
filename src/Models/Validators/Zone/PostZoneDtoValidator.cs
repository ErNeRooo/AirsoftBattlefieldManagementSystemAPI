using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Zone;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Models.Validators.ZoneVertex;
using FluentValidation;


namespace AirsoftBattlefieldManagementSystemAPI.Models.Validators.Zone;

public class PostZoneDtoValidator : AbstractValidator<PostZoneDto>
{
    public PostZoneDtoValidator(IBattleManagementSystemDbContext dbContext)
    {
        RuleFor(zone => zone.Name)
            .MaximumLength(50);
        
        RuleFor(zone => zone.Type)
            .NotEmpty()
            .Custom((value, context) =>
            {
                List<string> types = typeof(ZoneTypes)
                    .GetFields()
                    .Where(f => f.FieldType == typeof(string))
                    .Select(f => (string)f.GetRawConstantValue()!)
                    .ToList();

                if(!types.Contains(value)) context.AddFailure("Invalid type of zone");
            });

        RuleFor(zone => zone.BattleId)
            .NotEmpty()
            .Custom((value, context) =>
            {
                bool doesBattleIdExist = dbContext.Battle.Any(battle => battle.BattleId == value);
                
                if(!doesBattleIdExist) context.AddFailure($"Battle with id {value} does not exist");
            });

        RuleFor(zone => zone.Vertices).ForEach(zoneVertexRule =>
        {
            zoneVertexRule.SetValidator(new PostZoneVertexDtoValidator());
        });
        
        RuleFor(zone => zone.Vertices.Count)
            .NotEmpty()
            .GreaterThan(2)
            .LessThanOrEqualTo(20);
    }
}
