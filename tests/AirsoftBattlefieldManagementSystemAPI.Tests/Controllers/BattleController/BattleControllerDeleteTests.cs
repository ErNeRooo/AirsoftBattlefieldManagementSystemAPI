using System.Net;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Controllers.BattleController;

public class BattleControllerDeleteTests
{
    private HttpClient _client;
    private string _endpoint = "battle/id/";
    
    [Theory]
    [InlineData(1, 1)]
    [InlineData(3, 2)]
    public async void Delete_ValidId_ReturnsNoContent(int senderPlayerId, int battleId)
    {
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        var response = await _client.DeleteAsync($"{_endpoint}{battleId}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(112354)]
    public async void Delete_NotExistingBattle_ReturnsNotFound(int battleId)
    {
        var factory = new CustomWebApplicationFactory<Program>();
        _client = factory.CreateClient();
        
        var response = await _client.DeleteAsync($"{_endpoint}{battleId}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
    
    [Theory]
    [InlineData(2, 1)]
    [InlineData(2, 2)]
    [InlineData(4, 2)]
    [InlineData(5, 1)]
    [InlineData(5, 2)]
    [InlineData(6, 2)]
    [InlineData(6, 1)]
    public async void Delete_AsNotRoomAdmin_ReturnsForbidden(int senderPlayerId, int battleId)
    {
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        var response = await _client.DeleteAsync($"{_endpoint}{battleId}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}