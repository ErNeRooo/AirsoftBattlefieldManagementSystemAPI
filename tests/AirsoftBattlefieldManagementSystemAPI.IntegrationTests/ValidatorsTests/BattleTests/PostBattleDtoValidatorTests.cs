using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Battle;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Models.Validators.Battle;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;

namespace AirsoftBattlefieldManagementSystemAPI.IntegrationTests.ValidatorsTests.BattleTests;

public class PostBattleDtoValidatorTests
{
    private BattleManagementSystemDbContext _dbContext;
    
    public PostBattleDtoValidatorTests()
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
                JoinCode = "000000",
                PasswordHash = ""
            });
        _dbContext.SaveChanges();
    }
    
    [Theory]
    [InlineData("p", 1)]
    [InlineData("wda{{d\"T3$t\"ds@d}#w))(*&^a\"w!as-_=+/\\|%?'''", 1)]
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", 1)]
    public void Validate_ForValidModel_ReturnsSuccess(string name, int roomId)
    {
        var validator = new PostBattleDtoValidator(_dbContext);
        var model = new PostBattleDto
        {
            Name = name,
            RoomId = roomId,
            IsActive = false
        };
        
        var result = validator.TestValidate(model);
        
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Theory]
    [InlineData("", 1)]
    [InlineData("test", 2)]
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", 1)]
    public void Validate_ForInvalidModel_ReturnsFailure(string name, int roomId)
    {
        var validator = new PostBattleDtoValidator(_dbContext);
        var model = new PostBattleDto
        {
            Name = name,
            RoomId = roomId,
            IsActive = false
        };
        
        var result = validator.TestValidate(model);
        
        result.ShouldHaveAnyValidationError();
    }
}