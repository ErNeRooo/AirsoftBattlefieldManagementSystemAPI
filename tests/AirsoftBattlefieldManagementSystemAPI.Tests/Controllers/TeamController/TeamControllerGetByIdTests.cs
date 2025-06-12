using System.Net;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Team;
using AirsoftBattlefieldManagementSystemAPI.Tests.Helpers;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Controllers.TeamController;

public class TeamControllerGetByIdTests
{
    private HttpClient _client;
    private string _endpoint = "team/id/";
    
    [Theory]
    [InlineData(1, 1, "Red", 1, 1)]
    [InlineData(1, 2, "Blue", 1, 2)]
    [InlineData(2, 1, "Red", 1, 1)]
    [InlineData(3, 3, "Blue", 2, 3)]
    public async void GetById_ValidId_ReturnsOkAndTeamDto(int senderPlayerId, int teamId, string name, int roomId, int officerPlayerId)
    {
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        var response = await _client.GetAsync($"{_endpoint}{teamId}");
        var result = await response.Content.DeserializeFromHttpContentAsync<TeamDto>();
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        result.ShouldNotBeNull();
        result.TeamId.ShouldBe(teamId);
        result.Name.ShouldBe(name);
        result.RoomId.ShouldBe(roomId);
        result.OfficerPlayerId.ShouldBe(officerPlayerId);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(234641)]
    [InlineData(-1)]
    public async void GetById_TeamDoesNotExist_ReturnsNotFound(int id)
    {
        var factory = new CustomWebApplicationFactory<Program>();
        _client = factory.CreateClient();
        
        var response = await _client.GetAsync($"{_endpoint}{id}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
    
    [Theory]
    [InlineData(1, 3)]
    [InlineData(2, 3)]
    [InlineData(3, 1)]
    [InlineData(4, 2)]
    [InlineData(5, 1)]
    [InlineData(5, 2)]
    public async void GetById_TeamIsInDifferentRoom_ReturnsForbidden(int senderPlayerId, int teamId)
    {
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        var response = await _client.GetAsync($"{_endpoint}{teamId}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}