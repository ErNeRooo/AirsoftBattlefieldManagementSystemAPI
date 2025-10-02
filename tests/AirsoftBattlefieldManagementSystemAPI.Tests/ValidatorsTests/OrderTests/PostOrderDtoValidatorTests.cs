using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Order;
using AirsoftBattlefieldManagementSystemAPI.Models.Validators.Order;
using FluentValidation.TestHelper;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.ValidatorsTests.OrderTests;

public class PostOrderDtoValidatorTests
{
    public static IEnumerable<object[]> GetValidOrder()
    {
        var timestamp = DateTimeOffset.Now;
        var orders = new List<PostOrderDto>
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
        
        return orders.Select(x => new object[] { x });
    }
    
    public static IEnumerable<object[]> GetInvalidOrder()
    {
        var timestamp = DateTimeOffset.Now;
        var orders = new List<PostOrderDto>
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
                Type = "move"
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
        
        return orders.Select(x => new object[] { x });
    }
    
    [Theory]
    [MemberData(nameof(GetValidOrder))]
    public void Validate_ForValidModel_ReturnsSuccess(PostOrderDto model)
    {
        var validator = new PostOrderDtoValidator();

        var result = validator.TestValidate(model);
        
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Theory]
    [MemberData(nameof(GetInvalidOrder))]
    public void Validate_ForInvalidModel_ReturnsFailure(PostOrderDto model)
    {
        var validator = new PostOrderDtoValidator();

        var result = validator.TestValidate(model);
        
        result.ShouldHaveAnyValidationError();
    }
}