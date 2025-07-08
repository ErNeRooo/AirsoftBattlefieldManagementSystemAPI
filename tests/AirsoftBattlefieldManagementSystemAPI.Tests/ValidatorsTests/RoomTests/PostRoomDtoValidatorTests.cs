using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Room;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Models.Validators.Room;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.ValidatorsTests.RoomTests;

public class PostRoomDtoValidatorTests
{
    private BattleManagementSystemDbContext _dbContext;
    
    public PostRoomDtoValidatorTests()
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
    [InlineData("", 10)]
    [InlineData("000000", 10)]
    [InlineData("777777", 2)]
    [InlineData("koodoo", 100000)]
    [InlineData("k00d00", 100000)]
    public void Validate_ForValidModel_ReturnsSuccess(string joinCode, int maxPlayers)
    {
        var validator = new PostRoomDtoValidator(_dbContext);
        var model = new PostRoomDto()
        {
            JoinCode = joinCode,
            Password = "",
            MaxPlayers = maxPlayers
        };
        
        var result = validator.TestValidate(model);
        
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Theory]
    [InlineData("00/000", 5)]
    [InlineData("00+000", 5)]
    [InlineData("00 000", 5)]
    [InlineData("00:000", 5)]
    [InlineData("00;000", 5)]
    [InlineData("00'000", 5)]
    [InlineData("00\"000", 5)]
    [InlineData("00{000", 5)]
    [InlineData("00@000", 5)]
    [InlineData("00$000", 5)]
    [InlineData("00[000", 5)]
    [InlineData("00(000", 5)]
    [InlineData("00\\000", 5)]
    [InlineData("00,000", 5)]
    [InlineData("00.000", 5)]
    [InlineData("00=000", 5)]
    [InlineData("0cos00", -1)]
    [InlineData("213700", 5)]
    [InlineData("2137000", 15)]
    [InlineData("21370", 10)]
    [InlineData("000000", 100001)]
    public void Validate_ForInvalidModel_ReturnsFailure(string joinCode, int maxPlayers)
    {
        var validator = new PostRoomDtoValidator(_dbContext);
        var model = new PostRoomDto()
        {
            JoinCode = joinCode,
            Password = "",
            MaxPlayers = maxPlayers
        };
        
        var result = validator.TestValidate(model);
        
        result.ShouldHaveAnyValidationError();
    }
}