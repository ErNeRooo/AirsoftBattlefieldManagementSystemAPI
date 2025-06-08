using System.Net;
using AirsoftBattlefieldManagementSystemAPI.IntegrationTests.Helpers;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.IntegrationTests.Controllers.PlayerController;

public class PlayerControllerRegisterTests
{
    private HttpClient _client;

    public PlayerControllerRegisterTests()
    {
        CustomWebApplicationFactory<Program> factory = new CustomWebApplicationFactory<Program>();
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Register_ValidModel_ReturnsCreated()
    {
        // arrange
        var model = new PostPlayerDto
        {
            Name = "Cool Name"
        };
        
        // act
        var response = await _client.PostAsync($"/player/register", model.ToJsonHttpContent());

        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
    }
    
    [Fact]
    public async Task Register_NameIsEmpty_ReturnsBadRequest()
    {
        // arrange
        var model = new PostPlayerDto();
        
        // act
        var response = await _client.PostAsync($"/player/register", model.ToJsonHttpContent());

        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task Register_NameIsTooLong_ReturnsBadRequest()
    {
        // arrange
        var model = new PostPlayerDto
        {
            Name = "Cool Name is too long"
        };
        
        // act
        var response = await _client.PostAsync($"/player/register", model.ToJsonHttpContent());

        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
}