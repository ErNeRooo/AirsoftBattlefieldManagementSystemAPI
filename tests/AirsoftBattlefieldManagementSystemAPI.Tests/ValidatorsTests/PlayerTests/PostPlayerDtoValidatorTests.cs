using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Player;
using AirsoftBattlefieldManagementSystemAPI.Models.Validators.Player;
using FluentValidation.TestHelper;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.ValidatorsTests.PlayerTests;

public class PostPlayerDtoValidatorTests
{
    [Theory]
    [InlineData("a")]
    [InlineData("aA!@#$%^&*()_+-={}[]")]
    public void Validate_ForValidModel_ReturnsSuccess(string name)
    {
        var validator = new PostPlayerDtoValidator();
        var model = new PostPlayerDto
        {
            Name = name
        };
        
        var result = validator.TestValidate(model);

        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData("aaaaaaaaaaaaaaaaaaaaa")]
    public void Validate_ForInvalidModel_ReturnsFailure(string name)
    {
        var validator = new PostPlayerDtoValidator();
        var model = new PostPlayerDto
        {
            Name = name
        };
        
        var result = validator.TestValidate(model);

        result.ShouldHaveAnyValidationError();
    }
}