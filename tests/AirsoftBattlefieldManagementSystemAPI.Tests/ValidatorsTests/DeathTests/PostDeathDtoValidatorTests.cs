using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Death;
using AirsoftBattlefieldManagementSystemAPI.Models.Validators.Death;
using FluentValidation.TestHelper;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.ValidatorsTests.DeathTests;

public class PostDeathDtoValidatorTests
{
    public static IEnumerable<object[]> GetValidDeath()
    {
        var datetime = DateTime.Now;
        var deaths = new List<PostDeathDto>
        {
            new()
            {
                Latitude = 0,
                Longitude = 0,
                Accuracy = 0,
                Bearing = 0,
                Time = datetime
            },
            new()
            {
                Latitude = 1,
                Longitude = 1,
                Accuracy = 1,
                Bearing = 1,
                Time = datetime
            },
            new()
            {
                Latitude = 52.22999515733903m,
                Longitude = 21.010084229332218m,
                Accuracy = 10,
                Bearing = 180,
                Time = datetime
            },
            new()
            {
                Latitude = 90m,
                Longitude = 180m,
                Accuracy = 15,
                Bearing = 360,
                Time = datetime
            },
            new()
            {
                Latitude = -90m,
                Longitude = -180m,
                Accuracy = 7,
                Bearing = 34,
                Time = datetime
            }
        };
        
        return deaths.Select(x => new object[] { x });
    }
    
    public static IEnumerable<object[]> GetInvalidDeath()
    {
        var datetime = DateTime.Now;
        var deaths = new List<PostDeathDto>
        {
            new()
            {
                Latitude = 1,
                Longitude = 1,
                Accuracy = -1,
                Bearing = 1,
                Time = datetime,
            },
            new()
            {
                Latitude = -90.1m,
                Longitude = 1,
                Accuracy = 1,
                Bearing = 1,
                Time = datetime,
            },
            new()
            {
                Latitude = 90.1m,
                Longitude = 1,
                Accuracy = 1,
                Bearing = 1,
                Time = datetime,
            },
            new()
            {
                Latitude = 1,
                Longitude = 180.1m,
                Accuracy = 1,
                Bearing = 1,
                Time = datetime,
            },
            new()
            {
                Latitude = 1,
                Longitude = -180.1m,
                Accuracy = 1,
                Bearing = 1,
                Time = datetime,
            },
            new()
            {
                Latitude = 1,
                Longitude = 1,
                Accuracy = 1,
                Bearing = -1,
                Time = datetime,
            },
            new()
            {
                Latitude = 1,
                Longitude = 1,
                Accuracy = 1,
                Bearing = 361,
                Time = datetime,
            }
        };
        
        return deaths.Select(x => new object[] { x });
    }
    
    [Theory]
    [MemberData(nameof(GetValidDeath))]
    public void Validate_ForValidModel_ReturnsSuccess(PostDeathDto model)
    {
        var validator = new PostDeathDtoValidator();

        var result = validator.TestValidate(model);
        
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Theory]
    [MemberData(nameof(GetInvalidDeath))]
    public void Validate_ForInvalidModel_ReturnsFailure(PostDeathDto model)
    {
        var validator = new PostDeathDtoValidator();

        var result = validator.TestValidate(model);
        
        result.ShouldHaveAnyValidationError();
    }
}