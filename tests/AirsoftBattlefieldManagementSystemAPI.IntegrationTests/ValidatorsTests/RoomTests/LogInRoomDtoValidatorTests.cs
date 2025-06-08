using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Room;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Models.Validators.Room;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;

namespace AirsoftBattlefieldManagementSystemAPI.IntegrationTests.ValidatorsTests.RoomTests;

public class LogInRoomDtoValidatorTests
{
    private BattleManagementSystemDbContext _dbContext;
    
    public LogInRoomDtoValidatorTests()
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
        _dbContext.Room.Add(new Room()
        {
            RoomId = 2,
            JoinCode = "000000",
            PasswordHash = ""
        });
        _dbContext.Room.Add(new Room()
        {
            RoomId = 3,
            JoinCode = "55555",
            PasswordHash = ""
        });
        _dbContext.Room.Add(new Room()
        {
            RoomId = 4,
            JoinCode = "7777777",
            PasswordHash = ""
        });
        _dbContext.SaveChanges();
    }
    
    [Theory]
    [InlineData("213700")]
    [InlineData("000000")]
    public void Validate_ForValidModel_ReturnsSuccess(string joinCode)
    {
        var validator = new LoginRoomDtoValidator(_dbContext);
        var model = new LoginRoomDto()
        {
            JoinCode = joinCode,
            Password = ""
        };
        
        var result = validator.TestValidate(model);
        
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Theory]
    [InlineData("999999")]
    [InlineData("404404")]
    [InlineData("7777777")]
    [InlineData("55555")]
    [InlineData("")]
    public void Validate_ForInvalidModel_ReturnsFailure(string joinCode)
    {
        var validator = new LoginRoomDtoValidator(_dbContext);
        var model = new LoginRoomDto()
        {
            JoinCode = joinCode,
            Password = ""
        };
        
        var result = validator.TestValidate(model);
        
        result.ShouldHaveAnyValidationError();
    }
}