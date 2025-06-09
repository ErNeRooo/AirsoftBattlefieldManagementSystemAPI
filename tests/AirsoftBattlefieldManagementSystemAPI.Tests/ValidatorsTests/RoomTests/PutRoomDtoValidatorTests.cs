using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Room;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Models.Validators.Room;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.ValidatorsTests.RoomTests;

public class PutRoomDtoValidatorTests
{
    private BattleManagementSystemDbContext _dbContext;
    
    public PutRoomDtoValidatorTests()
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
        _dbContext.Player.Add(new Player()
        {
            PlayerId = 1,
            RoomId = 1,
            Name = "a",
        });
        _dbContext.SaveChanges();
    }
    
    [Theory]
    [InlineData("000000", 10, null)]
    [InlineData("777777", 2, null)]
    [InlineData("koodoo", 100000, null)]
    [InlineData("P@nZ3r", 100, null)]
    [InlineData("UuWWuU", null, null)]
    [InlineData("", 11, null)]
    [InlineData("", null, 1)]
    [InlineData("", null, null)]
    public void Validate_ForValidModel_ReturnsSuccess(string joinCode, int? maxPlayers, int? adminPlayerId)
    {
        var validator = new PutRoomDtoValidator(_dbContext);
        var model = new PutRoomDto()
        {
            JoinCode = joinCode,
            Password = "",
            MaxPlayers = maxPlayers,
            AdminPlayerId = adminPlayerId
        };
        
        var result = validator.TestValidate(model);
        
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Theory]
    [InlineData("", 1, null)]
    [InlineData("", 0, null)]
    [InlineData("", -1, null)]
    [InlineData("213700", 5, null)]
    [InlineData("2137000", 15, null)]
    [InlineData("21370", 10, null)]
    [InlineData("000000", 100001, null)]
    [InlineData("", null, 2)]
    public void Validate_ForInvalidModel_ReturnsFailure(string joinCode, int? maxPlayers, int? adminPlayerId)
    {
        var validator = new PutRoomDtoValidator(_dbContext);
        var model = new PutRoomDto()
        {
            JoinCode = joinCode,
            Password = "",
            MaxPlayers = maxPlayers,
            AdminPlayerId = adminPlayerId
        };
        
        var result = validator.TestValidate(model);
        
        result.ShouldHaveAnyValidationError();
    }
}