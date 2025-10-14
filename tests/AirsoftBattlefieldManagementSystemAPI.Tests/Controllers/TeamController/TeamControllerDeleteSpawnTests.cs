using System.Net;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Controllers.TeamController;

public class TeamControllerDeleteSpawnTests
{
    private HttpClient _client;
    private string _endpoint = "team/spawn/teamId/";
    
    [Theory]
    [InlineData(1, 1)]
    [InlineData(1, 2)]
    [InlineData(3, 3)]
    [InlineData(3, 4)]
    [InlineData(3, 6)]
    public async void DeleteSpawn_ValidId_ReturnsNoContent(int senderPlayerId, int teamId)
    {
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        var response = await _client.DeleteAsync($"{_endpoint}{teamId}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(34214)]
    public async void DeleteSpawn_NotExistingTeam_ReturnsNotFound(int teamId)
    {
        var factory = new CustomWebApplicationFactory<Program>();
        _client = factory.CreateClient();
        
        var response = await _client.DeleteAsync($"{_endpoint}{teamId}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
    
    [Theory]
    [InlineData(2, 1)]
    [InlineData(2, 2)]
    [InlineData(4, 3)]
    [InlineData(6, 3)]
    [InlineData(9, 6)]
    public async void DeleteSpawn_PlayerIsNotAdmin_ReturnsForbidden(int senderPlayerId, int teamId)
    {
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        var response = await _client.DeleteAsync($"{_endpoint}{teamId}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
    
    [Theory]
    [InlineData(1, 3)]
    [InlineData(1, 4)]
    [InlineData(1, 6)]
    [InlineData(3, 1)]
    [InlineData(3, 2)]
    public async void DeleteSpawn_PlayerIsNotInTheSameRoom_ReturnsForbidden(int senderPlayerId, int teamId)
    {
        // Arrange
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();

        // Act
        var response = await _client.DeleteAsync($"{_endpoint}{teamId}");
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}