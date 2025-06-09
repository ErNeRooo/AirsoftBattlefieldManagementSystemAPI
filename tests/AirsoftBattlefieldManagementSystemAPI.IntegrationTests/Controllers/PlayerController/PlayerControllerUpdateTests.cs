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

    [Theory]
    [InlineData("Chisato", true, 1)]
    [InlineData("Takina", false, 2)]
    public async Task Update_ValidModel_ReturnsOk(string name, bool isDead, int teamId)
    {
        // arrange
        var model = new PutPlayerDto
        {
            Name = name,
            TeamId = teamId,
            IsDead = isDead
        };
        
        // act
        var response = await _client.PutAsync($"/player/id/{1}", model.ToJsonHttpContent());
        var result = await response.Content.DeserializeFromHttpContentAsync<PlayerDto>();
        
        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        result.Name.ShouldBe(name);
        result.TeamId.ShouldBe(teamId);
        result.IsDead.ShouldBe(isDead);
    }
    
    [Theory]
    [InlineData(null, null, null)]
    [InlineData("Takina", null, null)]
    [InlineData(null, null, 2)]
    [InlineData(null, true, null)]
    public async Task Update_GivenOnlySpecificField_ReturnsOk(string? name, bool? isDead, int? teamId)
    {
        // arrange
        var model = new PutPlayerDto
        {
            Name = name,
            TeamId = teamId,
            IsDead = isDead
        };
        
        // act
        var response = await _client.PutAsync($"/player/id/{1}", model.ToJsonHttpContent());
        var result = await response.Content.DeserializeFromHttpContentAsync<PlayerDto>();
        
        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        result.PlayerId.ShouldBe(1);
        result.RoomId.ShouldBe(1);
        result.Name.ShouldBe(name ?? "Chisato");
        result.TeamId.ShouldBe(teamId ?? 1);
        result.IsDead.ShouldBe(isDead ?? false);
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
        
        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task Update_TeamIdFieldIsNotValid_ReturnsBadRequest()
    {
        // arrange
        var model = new 
        {
            Name = "Namae",
            TeamId = "haha XD",
            IsDead = true
        };
        
        // act
        var response = await _client.PutAsync($"/player/id/{1}", model.ToJsonHttpContent());
        
        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
}