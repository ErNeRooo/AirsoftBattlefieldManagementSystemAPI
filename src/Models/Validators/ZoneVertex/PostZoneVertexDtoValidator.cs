using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.ZoneVertex;
using FluentValidation;


namespace AirsoftBattlefieldManagementSystemAPI.Models.Validators.ZoneVertex;

public class PostZoneVertexDtoValidator : AbstractValidator<PostZoneVertexDto>
{
    public PostZoneVertexDtoValidator()
    {
        RuleFor(vertex => vertex.Latitude)
            .InclusiveBetween(-90, 90);
            
        RuleFor(vertex => vertex.Longitude)
            .InclusiveBetween(-180, 180);
    }
}
