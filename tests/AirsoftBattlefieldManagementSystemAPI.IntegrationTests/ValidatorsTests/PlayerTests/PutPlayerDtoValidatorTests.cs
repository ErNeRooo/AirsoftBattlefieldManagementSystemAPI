using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Player;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Models.Validators.Player;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;

namespace AirsoftBattlefieldManagementSystemAPI.IntegrationTests.ValidatorsTests.PlayerTests;

public class PutPlayerDtoValidatorTests
{
    private BattleManagementSystemDbContext _dbContext;
    
    public PutPlayerDtoValidatorTests()
    {
        var builder = new DbContextOptionsBuilder<BattleManagementSystemDbContext>();
        builder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        _dbContext = new BattleManagementSystemDbContext(builder.Options);
        Seed();
    }

    public void Seed()
    {
        _dbContext.Team.Add(new Team()
        {
            Name = "Test Team",
            TeamId = 1,
        });
        _dbContext.Team.Add(new Team()
        {
            Name = "Test Team",
            TeamId = 2,
        });
        _dbContext.SaveChanges();
    }
    
    [Theory]
    [InlineData("a", 1)]
    [InlineData("test", null)]
    [InlineData("", null)]
    [InlineData("aA!@#$%^&*()_+-={}[]", 1)]
    [InlineData("", 2)]
    public void Validate_ForValidModel_ReturnsSuccess(string name, int? teamId)
    {
        var validator = new PutPlayerDtoValidator(_dbContext);
        var model = new PutPlayerDto
        {
            Name = name,
            TeamId = teamId
        };
        
        var result = validator.TestValidate(model);

        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Theory]
    [InlineData("aaaaaaaaaaaaaaaaaaaaa", 1)]
    [InlineData("test", 3)]
    public void Validate_ForInvalidModel_ReturnsFailure(string name, int teamId)
    {
        var validator = new PutPlayerDtoValidator(_dbContext);
        var model = new PutPlayerDto
        {
            Name = name,
            TeamId = teamId
        };
        
        var result = validator.TestValidate(model);

        result.ShouldHaveAnyValidationError();
    }
}