using System.Net;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Team;
using AirsoftBattlefieldManagementSystemAPI.Tests.Helpers;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Controllers.TeamController;

public class TeamControllerCreateTests
{
    private HttpClient _client;
    private string _endpoint = "team/";
    
    public TeamControllerCreateTests()
    {
        CustomWebApplicationFactory<Program> factory = new CustomWebApplicationFactory<Program>();
        _client = factory.CreateClient();
    }
    
    [Theory]
    [InlineData(1, 1, "Green")]
    [InlineData(2, 1, "Black")]
    [InlineData(3, 2, "Red")]
    [InlineData(4, 2, "White")]
    public async void Create_ValidModel_ReturnsCreatedAndTeamDto(int senderPlayerId, int roomId, string name)
    {
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();

        var model = new PostTeamDto
        {
            Name = name,
            RoomId = roomId
        };
        
        var response = await _client.PostAsync($"{_endpoint}", model.ToJsonHttpContent());
        var result = await response.Content.DeserializeFromHttpContentAsync<TeamDto>();
        
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        result.TeamId.ShouldNotBe(0);
        result.Name.ShouldBe(name);
        result.RoomId.ShouldBe(roomId);
    }
    
    [Theory]
    [InlineData(1, 2)]
    [InlineData(2, 2)]
    [InlineData(3, 1)]
    [InlineData(4, 1)]
    [InlineData(5, 1)]
    [InlineData(5, 2)]
    public async void Create_RoomIdOfDifferentRoom_ReturnsForbidden(int senderPlayerId, int roomId)
    {
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();

        var model = new PostTeamDto
        {
            Name = "Nice Team",
            RoomId = roomId
        };
        
        var response = await _client.PostAsync($"{_endpoint}", model.ToJsonHttpContent());

        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}