using System.Net;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Team;
using AirsoftBattlefieldManagementSystemAPI.Tests.Helpers;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Controllers.TeamController;

public class TeamControllerUpdateTests
{
    private HttpClient _client;
    private string _endpoint = "team/id/";
    
    [Theory]
    [InlineData(1, 1, "White", 1)]
    [InlineData(1, 2, "White", 2)]
    [InlineData(2, 2, "White", 2)]
    [InlineData(3, 3, "White", 4)]
    public async void Update_ValidId_ReturnsOkAndTeamDto(int senderPlayerId, int teamId, string name, int officerPlayerId)
    {
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();

        var model = new PutTeamDto
        {
            Name = name,
            OfficerPlayerId = officerPlayerId
        };
        
        var response = await _client.PutAsync($"{_endpoint}{teamId}", model.ToJsonHttpContent());
        var result = await response.Content.DeserializeFromHttpContentAsync<TeamDto>();
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        result.ShouldNotBeNull();
        result.TeamId.ShouldBe(teamId);
        result.Name.ShouldBe(name);
        result.OfficerPlayerId.ShouldBe(officerPlayerId);
    }
    
    [Theory]
    [InlineData(1, 1, "White", null)]
    [InlineData(1, 1, null, null)]
    [InlineData(3, 3, null, 4)]
    [InlineData(3, 3, null, null)]
    public async void Update_NotAllFieldsSpecified_ReturnsOkAndTeamDto(int senderPlayerId, int teamId, string? name, int? officerPlayerId)
    {
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();

        var model = new PutTeamDto
        {
            Name = name,
            OfficerPlayerId = officerPlayerId
        };
        
        var responseFromGet = await _client.GetAsync($"team/id/{teamId}");
        var resultFromGet = await responseFromGet.Content.DeserializeFromHttpContentAsync<TeamDto>();
        
        responseFromGet.StatusCode.ShouldBe(HttpStatusCode.OK);
        
        var response = await _client.PutAsync($"{_endpoint}{teamId}", model.ToJsonHttpContent());
        var result = await response.Content.DeserializeFromHttpContentAsync<TeamDto>();
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        result.ShouldNotBeNull();
        result.TeamId.ShouldBe(teamId);
        result.Name.ShouldBe(name ?? resultFromGet.Name);
        result.OfficerPlayerId.ShouldBe(officerPlayerId ?? resultFromGet.OfficerPlayerId);
    }
    
    [Theory]
    [InlineData(-2)]
    [InlineData(0)]
    [InlineData(24138)]
    public async void Update_ForTeamThatDoesNotExist_ReturnsNotFound(int id)
    {
        var factory = new CustomWebApplicationFactory<Program>();
        _client = factory.CreateClient();
        
        var model = new PutTeamDto();
        
        var response = await _client.PutAsync($"{_endpoint}{id}", model.ToJsonHttpContent());
        
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Theory]
    [InlineData(1, 3)]
    [InlineData(3, 1)]
    [InlineData(3, 2)]
    public async void Update_ForTeamFromForeignRoom_ReturnsForbidden(int senderPlayerId, int teamId)
    {
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        var model = new PutTeamDto();
        
        var response = await _client.PutAsync($"{_endpoint}{teamId}", model.ToJsonHttpContent());
        
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }

    [Theory]
    [InlineData(2, 1)]
    [InlineData(4, 3)]
    public async void Update_NonOfficerOrAdminPlayer_ReturnsForbidden(int senderPlayerId, int teamId)
    {
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        var model = new PutTeamDto();
        
        var response = await _client.PutAsync($"{_endpoint}{teamId}", model.ToJsonHttpContent());
        
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
    
    [Theory]
    [InlineData(1, 1, 2)]
    [InlineData(2, 2, 3)]
    [InlineData(3, 2, 6)]
    public async void Update_OfficerPlayerIdSetToPlayerFromForeignTeam_ReturnsForbidden(int senderPlayerId, int teamId, int officerPlayerId)
    {
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        var model = new PutTeamDto
        {
            OfficerPlayerId = officerPlayerId,
        };
        
        var response = await _client.PutAsync($"{_endpoint}{teamId}", model.ToJsonHttpContent());
        
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}