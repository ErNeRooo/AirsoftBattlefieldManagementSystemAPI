using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Team;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Models.Validators.Team;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;

namespace AirsoftBattlefieldManagementSystemAPI.IntegrationTests.ValidatorsTests.TeamTests;

public class PutTeamDtoValidatorTests
{
    private BattleManagementSystemDbContext _dbContext;
    
    public PutTeamDtoValidatorTests()
    {
        var builder = new DbContextOptionsBuilder<BattleManagementSystemDbContext>();
        builder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        _dbContext = new BattleManagementSystemDbContext(builder.Options);
        Seed();
    }

    public void Seed()
    {
        _dbContext.Player.Add(new Player()
        {
            PlayerId = 1,
            Name = "aaa",
            RoomId = 1
        });
        _dbContext.SaveChanges();
    }

    [Theory]
    [InlineData("d", 1)]
    [InlineData("da", null)]
    [InlineData("", 1)]
    [InlineData("", null)]
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", 1)]
    public void Validate_ForValidModel_ReturnsSuccess(string name, int? officerPlayerId)
    {
        var validator = new PutTeamDtoValidator(_dbContext);
        var model = new PutTeamDto
        {
            Name = name,
            OfficerPlayerId = officerPlayerId
        };
        
        var result = validator.TestValidate(model);
        
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Theory]
    [InlineData("d", 0)]
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", 1)]
    public void Validate_ForInvalidModel_ReturnsFailure(string name, int? officerPlayerId)
    {
        var validator = new PutTeamDtoValidator(_dbContext);
        var model = new PutTeamDto
        {
            Name = name,
            OfficerPlayerId = officerPlayerId
        };
        
        var result = validator.TestValidate(model);
        
        result.ShouldHaveAnyValidationError();
    }
}