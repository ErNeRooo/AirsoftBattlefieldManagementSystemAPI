using System.Net;
using AirsoftBattlefieldManagementSystemAPI.IntegrationTests.Helpers;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Player;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.IntegrationTests.Controllers.PlayerController;

public class PlayerControllerUpdateTests
{
    private HttpClient _client;

    public PlayerControllerUpdateTests()
    {
        CustomWebApplicationFactory<Program> factory = new CustomWebApplicationFactory<Program>();
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Update_ValidModel_ReturnsOk()
    {
        // arrange
        var model = new PutPlayerDto
        {
            Name = "Name",
            TeamId = 1,
            IsDead = false
        };
        
        // act
        var response = await _client.PutAsync($"/player/id/{1}", model.ToJsonHttpContent());

        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task Update_GivenOnlySpecificField_ReturnsOk()
    {
        // arrange
        var model = new PutPlayerDto
        {
            Name = "Namae",
        };
        
        // act
        var response = await _client.PutAsync($"/player/id/{1}", model.ToJsonHttpContent());
        
        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task Update_IsDeadFieldIsNotValid_ReturnsBadRequest()
    {
        // arrange
        var model = new 
        {
            Name = "Namae",
            TeamId = 1,
            IsDead = "xd"
        };
        
        // act
        var response = await _client.PutAsync($"/player/id/{1}", model.ToJsonHttpContent());
        var a = response.Content.ReadAsStringAsync().Result;
        
        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
}