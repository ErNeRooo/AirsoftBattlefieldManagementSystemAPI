using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Zone;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.ZoneVertex;
using AirsoftBattlefieldManagementSystemAPI.Models.Validators.Zone;
using AirsoftBattlefieldManagementSystemAPI.Models.Validators.ZoneVertex;
using FluentValidation.TestHelper;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.ValidatorsTests.ZoneVertexTests;

public class PostZoneVertexDtoValidatorTests
{
    public static IEnumerable<object[]> GetValidZoneVertices()
    {
        var vertices = new List<PostZoneVertexDto>
        {
            new (0,0),
            new (180,90),
            new (-180,-90),
            new (23, 41),
            new (0.42134m, 57.4535234125m),
            new (-123,-86),
        };
        
        return vertices.Select(x => new object[] { x });
    }
    
    public static IEnumerable<object[]> GetInvalidZoneVertices()
    {
        var vertices = new List<PostZoneVertexDto>
        {
            new (180.01m,1),
            new (-180.01m,1),
            new (1,-90.01m),
            new (1,90.01m),
            new (1,91),
            new (1234,9),
            new (-01340,3),
            new (12,91123),
            new (1234,-654),
        };
        
        return vertices.Select(x => new object[] { x });
    }
    
    [Theory]
    [MemberData(nameof(GetValidZoneVertices))]
    public void Validate_ForValidModel_ReturnsSuccess(PostZoneVertexDto model)
    {
        var validator = new PostZoneVertexDtoValidator();

        var result = validator.TestValidate(model);
        
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Theory]
    [MemberData(nameof(GetInvalidZoneVertices))]
    public void Validate_ForInvalidModel_ReturnsFailure(PostZoneVertexDto model)
    {
        var validator = new PostZoneVertexDtoValidator();

        var result = validator.TestValidate(model);
        
        result.ShouldHaveAnyValidationError();
    }
}