using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Location;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Models.Validators.Location;
using FluentValidation.TestHelper;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.ValidatorsTests.LocationTests;

public class PostLocationDtoValidatorTests
{
    public static IEnumerable<object[]> GetValidLocation()
    {
        var datetime = DateTime.Now;
        var locations = new List<PostLocationDto>
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
        
        return locations.Select(x => new object[] { x });
    }
    
    public static IEnumerable<object[]> GetInvalidLocation()
    {
        var datetime = DateTime.Now;
        var locations = new List<PostLocationDto>
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
        
        return locations.Select(x => new object[] { x });
    }
    
    [Theory]
    [MemberData(nameof(GetValidLocation))]
    public void Validate_ForValidModel_ReturnsSuccess(PostLocationDto model)
    {
        var validator = new PostLocationDtoValidator();

        var result = validator.TestValidate(model);
        
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Theory]
    [MemberData(nameof(GetInvalidLocation))]
    public void Validate_ForInvalidModel_ReturnsFailure(PostLocationDto model)
    {
        var validator = new PostLocationDtoValidator();

        var result = validator.TestValidate(model);
        
        result.ShouldHaveAnyValidationError();
    }
}