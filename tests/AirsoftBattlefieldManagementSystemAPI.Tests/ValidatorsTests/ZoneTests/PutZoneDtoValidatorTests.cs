using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Zone;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.ZoneVertex;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Models.Validators.Zone;
using FluentValidation.TestHelper;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.ValidatorsTests.ZoneTests;

public class PutZoneDtoValidatorTests
{
    public static IEnumerable<object[]> GetValidZone()
    {
        var zones = new List<PutZoneDto>
        {
            new ()
            {
                Name = "Test Zone",
                Type = ZoneTypes.NO_FIRE_ZONE
            },
            new ()
            {
                Name = "!@#$%^&*()-\\/[]{}();:'\"",
                Type = ZoneTypes.SPAWN
            },
            new ()
            {
                Name = "",
                Type = "SPAWN"
            },
            new ()
            {
                Name = "eqaw",
                Type = ""
            },
        };
        
        return zones.Select(x => new object[] { x });
    }
    
    public static IEnumerable<object[]> GetInvalidZone()
    {
        var zones = new List<PutZoneDto>
        {
            new ()
            {
                Name = "oWo",
                Type = "spawn"
            },
            new ()
            {
                Name = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
                Type = "SPAWN"
            }
        };
        
        return zones.Select(x => new object[] { x });
    }
    
    [Theory]
    [MemberData(nameof(GetValidZone))]
    public void Validate_ForValidModel_ReturnsSuccess(PutZoneDto model)
    {
        var validator = new PutZoneDtoValidator();

        var result = validator.TestValidate(model);
        
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Theory]
    [MemberData(nameof(GetInvalidZone))]
    public void Validate_ForInvalidModel_ReturnsFailure(PutZoneDto model)
    {
        var validator = new PutZoneDtoValidator();

        var result = validator.TestValidate(model);
        
        result.ShouldHaveAnyValidationError();
    }
}