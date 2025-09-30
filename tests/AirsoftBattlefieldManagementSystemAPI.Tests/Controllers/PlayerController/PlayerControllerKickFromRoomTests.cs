using System.Net;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Player;
using AirsoftBattlefieldManagementSystemAPI.Tests.Helpers;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Controllers.PlayerController;

public class PlayerControllerKickFromRoomTests
{
    private HttpClient _client;
    private string _endpoint = "player/kick-from-room/playerId/";
    
    [Theory]
    [InlineData(1, 1)]
    [InlineData(1, 2)]
    public async Task Kick_ValidId_ReturnsOkAndPlayerDto(int senderPlayerId, int targetPlayerId)
    {
        CustomWebApplicationFactory<Program> factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        var responseFromGet = await _client.GetAsync($"/player/id/{targetPlayerId}");
        var resultFromGet = await responseFromGet.Content.DeserializeFromHttpContentAsync<PlayerDto>();
        
        responseFromGet.StatusCode.ShouldBe(HttpStatusCode.OK);
        
        var response = await _client.PostAsync($"{_endpoint}{targetPlayerId}", null);
        var result = await response.Content.DeserializeFromHttpContentAsync<PlayerDto>();
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        result.ShouldNotBeNull();
        result.PlayerId.ShouldBe(resultFromGet.PlayerId);
        result.TeamId.ShouldBe(0);
        result.RoomId.ShouldBe(0);
        result.IsDead.ShouldBe(resultFromGet.IsDead);
        result.Name.ShouldBe(resultFromGet.Name);
    }
    
    [Theory]
    [InlineData(4, 3)]
    [InlineData(4, 4)]
    public async Task Kick_SenderPlayerIsNotAdmin_ReturnsForbidden(int senderPlayerId, int targetPlayerId)
    {
        CustomWebApplicationFactory<Program> factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        var response = await _client.PostAsync($"{_endpoint}{targetPlayerId}", null);
        
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
    
    [Theory]
    [InlineData("-123")]
    [InlineData("0")]
    [InlineData("")]
    [InlineData("12314")]
    public async Task Kick_PlayerWithGivenIdNotFound_ReturnsNotFound(string id)
    {
        CustomWebApplicationFactory<Program> factory = new CustomWebApplicationFactory<Program>();
        _client = factory.CreateClient();
        
        var response = await _client.PostAsync($"{_endpoint}{id}", null);
        
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}