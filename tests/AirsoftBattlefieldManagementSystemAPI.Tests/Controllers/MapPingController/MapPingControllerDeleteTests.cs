using System.Net;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Controllers.MapPingController;

public class MapPingControllerDeleteTests
{
    private HttpClient _client;
    private string _endpoint = "map-ping/id/";
        
    [Theory]
    [InlineData(4, 4)]
    [InlineData(10, 10)]
    [InlineData(11, 11)]
    public async void Delete_AsNotOfficer_ReturnsNoContent(int senderPlayerId, int mapPingId)
    {
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        var response = await _client.DeleteAsync($"{_endpoint}{mapPingId}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
    
    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 2)]
    [InlineData(3, 4)]
    [InlineData(9, 10)]
    [InlineData(9, 11)]
    public async void Delete_AsOfficer_ReturnsNoContent(int senderPlayerId, int mapPingId)
    {
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        var response = await _client.DeleteAsync($"{_endpoint}{mapPingId}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(112354)]
    public async void Delete_NotExistingMapPing_ReturnsNotFound(int mapPingId)
    {
        var factory = new CustomWebApplicationFactory<Program>();
        _client = factory.CreateClient();
        
        var response = await _client.DeleteAsync($"{_endpoint}{mapPingId}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
    
    [Theory]
    [InlineData(10, 11)]
    [InlineData(11, 10)]
    [InlineData(10, 9)]
    [InlineData(4, 3)]
    public async void Delete_MapPingOfOtherPlayerInTheSameTeamAsNotOfficer_ReturnsForbidden(int senderPlayerId, int mapPingId)
    {
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        var response = await _client.DeleteAsync($"{_endpoint}{mapPingId}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
    
    [Theory]
    [InlineData(1, 2)]
    [InlineData(1, 3)]
    [InlineData(1, 9)]
    [InlineData(1, 11)]
    [InlineData(3, 9)]
    [InlineData(3, 11)]
    [InlineData(4, 9)]
    [InlineData(4, 10)]
    [InlineData(9, 4)]
    public async void Delete_MapPingOfPlayerFromOtherTeam_ReturnsForbidden(int senderPlayerId, int mapPingId)
    {
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        var response = await _client.DeleteAsync($"{_endpoint}{mapPingId}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}