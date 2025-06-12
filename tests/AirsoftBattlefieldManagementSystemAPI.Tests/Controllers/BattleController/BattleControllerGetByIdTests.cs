using System.Net;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Battle;
using AirsoftBattlefieldManagementSystemAPI.Tests.Helpers;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Controllers.BattleController;

public class BattleControllerGetByIdTests
{
    private HttpClient _client;
    private string _endpoint = "battle/id/";
    
    [Theory]
    [InlineData(1, 1, "Kursk", false, 1)]
    [InlineData(4, 2, "Rhine", true, 2)]
    public async void GetById_ValidId_ReturnsOkAndBattleDto(int senderPlayerId, int battleId, string name, bool isActive, int roomId)
    {
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        var response = await _client.GetAsync($"{_endpoint}{battleId}");
        var result = await response.Content.DeserializeFromHttpContentAsync<BattleDto>();
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        result.ShouldNotBeNull();
        result.BattleId.ShouldBe(battleId);
        result.Name.ShouldBe(name);
        result.IsActive.ShouldBe(isActive);
        result.RoomId.ShouldBe(roomId);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(234641)]
    [InlineData(-1)]
    public async void GetById_BattleDoesNotExist_ReturnsNotFound(int id)
    {
        var factory = new CustomWebApplicationFactory<Program>();
        _client = factory.CreateClient();
        
        var response = await _client.GetAsync($"{_endpoint}{id}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
    
    [Theory]
    [InlineData(1, 2)]
    [InlineData(2, 2)]
    [InlineData(3, 1)]
    [InlineData(4, 1)]
    [InlineData(5, 2)]
    [InlineData(5, 1)]
    public async void GetById_BattleIsInDifferentRoom_ReturnsForbidden(int senderPlayerId, int battleId)
    {
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        var response = await _client.GetAsync($"{_endpoint}{battleId}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}