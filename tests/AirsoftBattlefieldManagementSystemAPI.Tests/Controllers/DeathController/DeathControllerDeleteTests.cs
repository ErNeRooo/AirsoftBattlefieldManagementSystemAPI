using System.Net;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Controllers.DeathController;

public class DeathControllerDeleteTests
{
    private HttpClient _client;
    private string _endpoint = "death/id/";

    public DeathControllerDeleteTests()
    {
        
        CustomWebApplicationFactory<Program> factory = new CustomWebApplicationFactory<Program>();
        _client = factory.CreateClient();
    }
    
        
    [Theory]
    [InlineData(1, 1)]
    [InlineData(1, 6)]
    [InlineData(2, 3)]
    [InlineData(3, 5)]
    [InlineData(4, 7)]
    public async void Delete_ValidId_ReturnsNoContent(int senderPlayerId, int deathId)
    {
        CustomWebApplicationFactory<Program> factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        var response = await _client.DeleteAsync($"{_endpoint}{deathId}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(112354)]
    public async void Delete_NotExistingDeath_ReturnsNotFound(int deathId)
    {
        var response = await _client.DeleteAsync($"{_endpoint}{deathId}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
    
    [Theory]
    [InlineData(1, 3)]
    [InlineData(2, 1)]
    [InlineData(3, 7)]
    [InlineData(4, 5)]
    [InlineData(4, 1)]
    [InlineData(1, 7)]
    [InlineData(6, 4)]
    [InlineData(5, 4)]
    [InlineData(5, 7)]
    public async void Delete_AsOtherPlayer_ReturnsForbidden(int senderPlayerId, int deathId)
    {
        CustomWebApplicationFactory<Program> factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        var response = await _client.DeleteAsync($"{_endpoint}{deathId}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}