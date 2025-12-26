using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Models.Helpers;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.UnitTests;

public class RoomEntityHelperTests
{
    private readonly Room _room = new Room()
    {
        Players = new List<Player>()
        {
            new Player() { TeamId = 1, PlayerId = 1 },
            new Player() { TeamId = 1, PlayerId = 2 },
            new Player() { TeamId = 1, PlayerId = 3 },
            new Player() { TeamId = 1, PlayerId = 4 },
            new Player() { TeamId = 2, PlayerId = 5 },
            new Player() { TeamId = null, PlayerId = 6 },
            new Player() { TeamId = null, PlayerId = 7 }
        }
    };
    
    [Fact]
    public void GetTeamPlayerIdsWithoutSelf_ReturnsCorrectIds()
    {
        List<string> ids = _room.GetTeamPlayerIdsWithoutSelf(1, 3);
        
        ids.ShouldBe(new[] { "1", "2", "4" });
    }
    
    [Fact]
    public void GetTeamPlayerIdsWithoutSelf_TeamIsNull_ReturnsCorrectIds()
    {
        List<string> ids = _room.GetTeamPlayerIdsWithoutSelf(null, 6);
        
        ids.ShouldBe(new[] { "7" });
    }
    
    [Fact]
    public void GetAllPlayerIdsWithoutSelf_Test()
    {
        List<string> ids = _room.GetAllPlayerIdsWithoutSelf(3);
        
        ids.ShouldBe(new[] { "1", "2", "4", "5", "6", "7" });
    }
    
    [Fact]
    public void GetAllPlayerIdsWithoutSelf_SelfPlayerHasNoTeam_ReturnsCorrectIds()
    {
        List<string> ids = _room.GetAllPlayerIdsWithoutSelf(6);
        
        ids.ShouldBe(new[] { "1", "2", "3", "4", "5", "7" });
    }
}