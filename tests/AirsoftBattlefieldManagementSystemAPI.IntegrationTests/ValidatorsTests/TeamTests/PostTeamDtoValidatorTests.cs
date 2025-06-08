using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Team;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Models.Validators.Team;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;

namespace AirsoftBattlefieldManagementSystemAPI.IntegrationTests.ValidatorsTests.TeamTests;

public class PostTeamDtoValidatorTests
{
    private BattleManagementSystemDbContext _dbContext;
    
    public PostTeamDtoValidatorTests()
    {
        var builder = new DbContextOptionsBuilder<BattleManagementSystemDbContext>();
        builder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        _dbContext = new BattleManagementSystemDbContext(builder.Options);
        Seed();
    }

    public void Seed()
    {
        _dbContext.Room.Add(new Room()
        {
            RoomId = 1,
            JoinCode = "213700",
            PasswordHash = ""
        });
        _dbContext.SaveChanges();
    }

    [Theory]
    [InlineData("d", 1)]
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", 1)]
    public void Validate_ForValidModel_ReturnsSuccess(string name, int roomId)
    {
        var validator = new PostTeamDtoValidator(_dbContext);
        var model = new PostTeamDto
        {
            Name = name,
            RoomId = roomId
        };
        
        var result = validator.TestValidate(model);
        
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Theory]
    [InlineData("d", 0)]
    [InlineData("", 1)]
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", 1)]
    public void Validate_ForInvalidModel_ReturnsFailure(string name, int roomId)
    {
        var validator = new PostTeamDtoValidator(_dbContext);
        var model = new PostTeamDto
        {
            Name = name,
            RoomId = roomId
        };
        
        var result = validator.TestValidate(model);
        
        result.ShouldHaveAnyValidationError();
    }
}