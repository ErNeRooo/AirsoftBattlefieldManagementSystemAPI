using System.Net;
using AirsoftBattlefieldManagementSystemAPI.Tests.Helpers;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Account;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Player;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Controllers.PlayerController;

public class PlayerControllerGetPlayerByIdTests
{
    private HttpClient _client;

    public PlayerControllerGetPlayerByIdTests()
    {
        CustomWebApplicationFactory<Program> factory = new CustomWebApplicationFactory<Program>();
        _client = factory.CreateClient();
    }

    [Theory]
    [InlineData(1, 1, 1, false, "Chisato")]
    [InlineData(2, 2, 1, false, "Takina")]
    public async Task GetById_ValidId_ReturnsOk(int playerId, int teamId, int roomId, bool isDead, string name)
    {
        var response = await _client.GetAsync($"/player/id/{playerId}");
        var result = await response.Content.DeserializeFromHttpContentAsync<PlayerDto>();
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        result.ShouldNotBeNull();
        result.PlayerId.ShouldBe(playerId);
        result.TeamId.ShouldBe(teamId);
        result.RoomId.ShouldBe(roomId);
        result.IsDead.ShouldBe(isDead);
        result.Name.ShouldBe(name);
    }
    
    [Fact]
    public async Task GetById_PlayerFromDifferentRoom_ReturnsForbidden()
    {
        var response = await _client.GetAsync($"/player/id/{3}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task GetById_PlayerDoesNotExists_ReturnsNotFound()
    {
        var response = await _client.GetAsync($"/player/id/{43535}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}