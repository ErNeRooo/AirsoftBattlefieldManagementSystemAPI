using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.MapPing;
using AirsoftBattlefieldManagementSystemAPI.Models.Validators.MapPing;
using FluentValidation.TestHelper;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.ValidatorsTests.MapPingTests;

public class PostMapPingDtoValidatorTests
{
    public static IEnumerable<object[]> GetValidMapPing()
    {
        var timestamp = DateTimeOffset.Now;
        var mapPings = new List<PostMapPingDto>
        {
            new()
            {
                Latitude = 0,
                Longitude = 0,
                Accuracy = 0,
                Bearing = 0,
                Time = timestamp
            },
            new()
            {
                Latitude = 1,
                Longitude = 1,
                Accuracy = 1,
                Bearing = 1,
                Time = timestamp
            },
            new()
            {
                Latitude = 90,
                Longitude = 180,
                Accuracy = 1,
                Bearing = 1,
                Time = timestamp
            },
            new()
            {
                Latitude = -90,
                Longitude = -180,
                Accuracy = 1,
                Bearing = 1,
                Time = timestamp
            },
            new()
            {
                Latitude = 90,
                Longitude = 180,
                Accuracy = 1,
                Bearing = 1,
                Time = timestamp
            },
            new()
            {
                Latitude = 52.22999515733903m,
                Longitude = 21.010084229332218m,
                Accuracy = 10,
                Bearing = 180,
                Time = timestamp
            },
            new()
            {
                Latitude = 90m,
                Longitude = 180m,
                Accuracy = 15,
                Bearing = 360,
                Time = timestamp
            },
            new()
            {
                Latitude = -90m,
                Longitude = -180m,
                Accuracy = 7,
                Bearing = 34,
                Time = timestamp
            }
        };
        
        return mapPings.Select(x => new object[] { x });
    }
    
    public static IEnumerable<object[]> GetInvalidMapPing()
    {
        var timestamp = DateTimeOffset.Now;
        var mapPings = new List<PostMapPingDto>
        {
            new()
            {
                Latitude = 1,
                Longitude = 1,
                Accuracy = -1,
                Bearing = 1,
                Time = timestamp,
            },
            new()
            {
                Latitude = -90.1m,
                Longitude = 1,
                Accuracy = 1,
                Bearing = 1,
                Time = timestamp,
            },
            new()
            {
                Latitude = 90.1m,
                Longitude = 1,
                Accuracy = 1,
                Bearing = 1,
                Time = timestamp,
            },
            new()
            {
                Latitude = 1,
                Longitude = 180.1m,
                Accuracy = 1,
                Bearing = 1,
                Time = timestamp,
            },
            new()
            {
                Latitude = 1,
                Longitude = -180.1m,
                Accuracy = 1,
                Bearing = 1,
                Time = timestamp,
            },
            new()
            {
                Latitude = 1,
                Longitude = 1,
                Accuracy = 1,
                Bearing = -1,
                Time = timestamp,
            },
            new()
            {
                Latitude = 1,
                Longitude = 1,
                Accuracy = 1,
                Bearing = 361,
                Time = timestamp,
            },
            new()
            {
                Latitude = 1,
                Longitude = 1,
                Accuracy = 1,
                Bearing = 361,
                Time = timestamp,
                Type = "ue"
            },
            new()
            {
                Latitude = 1,
                Longitude = 1,
                Accuracy = 1,
                Bearing = 3,
                Time = timestamp,
                Type = "enemy"
            },
            new()
            {
                Latitude = 1,
                Longitude = 1,
                Accuracy = 1,
                Bearing = 3,
                Time = timestamp,
                Type = "MOVE"
            },
            new()
            {
                Latitude = 1,
                Longitude = 1,
                Accuracy = 1,
                Bearing = 36,
                Time = timestamp,
                Type = "adwadasdedad"
            },
            new()
            {
                Latitude = 1,
                Longitude = 1,
                Accuracy = 1,
                Bearing = 31,
                Time = timestamp,
                Type = "213ddw"
            }
        };
        
        return mapPings.Select(x => new object[] { x });
    }
    
    [Theory]
    [MemberData(nameof(GetValidMapPing))]
    public void Validate_ForValidModel_ReturnsSuccess(PostMapPingDto model)
    {
        var validator = new PostMapPingDtoValidator();

        var result = validator.TestValidate(model);
        
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Theory]
    [MemberData(nameof(GetInvalidMapPing))]
    public void Validate_ForInvalidModel_ReturnsFailure(PostMapPingDto model)
    {
        var validator = new PostMapPingDtoValidator();

        var result = validator.TestValidate(model);
        
        result.ShouldHaveAnyValidationError();
    }
}