using System.Net;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Controllers.PlayerController;

public class PlayerControllerDeleteTests
{
    private HttpClient _client;
    private string _endpoint = "player";

    public PlayerControllerDeleteTests()
    {
        CustomWebApplicationFactory<Program> factory = new CustomWebApplicationFactory<Program>();
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Delete_ValidId_ReturnsNoContent()
    {
        // act
        var response = await _client.DeleteAsync($"{_endpoint}");
        
        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    } 
}