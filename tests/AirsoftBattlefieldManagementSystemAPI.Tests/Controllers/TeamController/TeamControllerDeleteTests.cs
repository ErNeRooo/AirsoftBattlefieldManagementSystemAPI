using System.Net;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Controllers.TeamController;

public class TeamControllerDeleteTests
{
    private HttpClient _client;
    private string _endpoint = "team/id/";
    
    [Theory]
    [InlineData(1, 1)]
    [InlineData(1, 2)]
    [InlineData(2, 2)]
    [InlineData(3, 3)]
    [InlineData(3, 4)]
    [InlineData(6, 4)]
    public async void Delete_ValidId_ReturnsNoContent(int senderPlayerId, int teamId)
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
    public async void Delete_NotExistingTeam_ReturnsNotFound(int teamId)
    {
        var factory = new CustomWebApplicationFactory<Program>();
        _client = factory.CreateClient();
        
        var response = await _client.DeleteAsync($"{_endpoint}{teamId}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
    // Delete_PlayerIsNotAdminOrOfficer_ReturnsForbidden
    
    [Theory]
    [InlineData(2, 1)]
    [InlineData(6, 3)]
    [InlineData(4, 4)]
    [InlineData(5, 1)]
    [InlineData(5, 4)]
    public async void Delete_PlayerIsNotAdminOrOfficer_ReturnsForbidden(int senderPlayerId, int teamId)
    {
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        var response = await _client.DeleteAsync($"{_endpoint}{teamId}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}