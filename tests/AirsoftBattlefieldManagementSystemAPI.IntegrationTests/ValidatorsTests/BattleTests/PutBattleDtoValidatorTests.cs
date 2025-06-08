using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Battle;
using AirsoftBattlefieldManagementSystemAPI.Models.Validators.Battle;
using FluentValidation.TestHelper;

namespace AirsoftBattlefieldManagementSystemAPI.IntegrationTests.ValidatorsTests.BattleTests;

public class PutBattleDtoValidatorTests
{
    [Theory]
    [InlineData("")]
    [InlineData("p")]
    [InlineData("wda{{d\"T3$t\"ds@d}#w))(*&^a\"w!as-_=+/\\|%?'''")]
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
    public void Validate_ForValidModel_ReturnsSuccess(string name)
    {
        var validator = new PutBattleDtoValidator();
        var model = new PutBattleDto
        {
            Name = name,
            IsActive = false
        };
        
        var result = validator.TestValidate(model);
        
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Theory]
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
    public void Validate_ForInvalidModel_ReturnsFailure(string name)
    {
        var validator = new PutBattleDtoValidator();
        var model = new PutBattleDto
        {
            Name = name,
            IsActive = false
        };
        
        var result = validator.TestValidate(model);
        
        result.ShouldHaveAnyValidationError();
    }
}