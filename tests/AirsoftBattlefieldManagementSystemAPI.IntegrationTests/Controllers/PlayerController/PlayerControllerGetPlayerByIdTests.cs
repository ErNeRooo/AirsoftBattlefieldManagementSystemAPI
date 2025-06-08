using System.Net;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.IntegrationTests.Controllers.PlayerController;

public class PlayerControllerGetPlayerByIdTests
{
    private HttpClient _client;

    public PlayerControllerGetPlayerByIdTests()
    {
        CustomWebApplicationFactory<Program> factory = new CustomWebApplicationFactory<Program>();
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetById_ValidId_ReturnsOk()
    {
        // act
        var response = await _client.GetAsync($"/player/id/{1}");

        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetById_IdOfDifferentPlayer_ReturnsOk()
    {
        // act
        var response = await _client.GetAsync($"/player/id/{2}");

        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task GetById_PlayerFromDifferentRoom_ReturnsForbidden()
    {
        // act
        var response = await _client.GetAsync($"/player/id/{3}");

        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task GetById_PlayerDoesNotExists_ReturnsNotFound()
    {
        // act
        var response = await _client.GetAsync($"/player/id/{43535}");

        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task GetById_InvalidId_ReturnsBadRequest()
    {
        // act
        var response = await _client.GetAsync($"/player/id/{"two"}");

        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
}