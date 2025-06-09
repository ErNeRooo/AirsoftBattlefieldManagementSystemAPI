using System.Net;
using AirsoftBattlefieldManagementSystemAPI.Tests.Helpers;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Player;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Controllers.PlayerController;

public class PlayerControllerRegisterTests
{
    private HttpClient _client;

    public PlayerControllerRegisterTests()
    {
        CustomWebApplicationFactory<Program> factory = new CustomWebApplicationFactory<Program>();
        _client = factory.CreateClient();
    }

    [Theory]
    [InlineData("ErNeRooo")]
    public async Task Register_ValidModel_ReturnsCreatedAndAccountDto(string name)
    {
        // arrange
        var model = new PostPlayerDto
        {
            Name = name
        };
        
        // act
        var response = await _client.PostAsync($"/player/register", model.ToJsonHttpContent());

        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        response.Headers.Location.ShouldNotBeNull();
        response.Content.ReadAsStringAsync().Result.ShouldNotBeNullOrEmpty();
    }
}