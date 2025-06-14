using System.Net;
using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Tests.Helpers;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Player;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Controllers.PlayerController;

public class PlayerControllerUpdateTests
{
    private HttpClient _client;
    private readonly string _endpoint = "player";

    [Theory]
    [InlineData(4, "Chisato", true, 3)]
    [InlineData(4, "Takina", false, 4)]
    public async Task Update_ValidModel_ReturnsOk(int senderPlayerId, string name, bool isDead, int teamId)
    {
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        var model = new PutPlayerDto
        {
            Name = name,
            TeamId = teamId,
            IsDead = isDead
        };
        
        var response = await _client.PutAsync($"{_endpoint}", model.ToJsonHttpContent());
        var result = await response.Content.DeserializeFromHttpContentAsync<PlayerDto>();
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        result.Name.ShouldBe(name);
        result.TeamId.ShouldBe(teamId);
        result.IsDead.ShouldBe(isDead);
    }
    
    [Theory]
    [InlineData(2,1)]
    [InlineData(1,2)]
    [InlineData(3,4)]
    [InlineData(6,3)]
    public async Task Update_WhenPlayerIsTeamOfficerAndSwitchesTeam_ReturnsOkAndUpdatesTeam(int senderPlayerId, int teamId)
    {
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        var model = new PutPlayerDto
        {
            TeamId = teamId,
        };
        
        using var scope = factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BattleManagementSystemDbContext>();
        
        var player = context.Player.FirstOrDefault(p => p.PlayerId == senderPlayerId);
        
        var response = await _client.PutAsync($"{_endpoint}", model.ToJsonHttpContent());
        var team = context.Team.FirstOrDefault(t => t.TeamId == player.TeamId);
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        team.OfficerPlayerId.ShouldBeNull();
    }
    
    [Theory]
    [InlineData(3)]
    public async Task Update_GivenTeamIdOfForeignRoom_ReturnsForbidden(int teamId)
    {
        var factory = new CustomWebApplicationFactory<Program>();
        _client = factory.CreateClient();
        
        var model = new PutPlayerDto
        {
            TeamId = teamId,
        };
        
        var response = await _client.PutAsync($"{_endpoint}", model.ToJsonHttpContent());
        
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
    
    [Theory]
    [InlineData(null, null, null)]
    [InlineData("Takina", null, null)]
    [InlineData(null, null, 2)]
    [InlineData(null, true, null)]
    public async Task Update_GivenOnlySpecificField_ReturnsOk(string? name, bool? isDead, int? teamId)
    {
        var factory = new CustomWebApplicationFactory<Program>();
        _client = factory.CreateClient();
        
        var model = new PutPlayerDto
        {
            Name = name,
            TeamId = teamId,
            IsDead = isDead
        };
        
        var response = await _client.PutAsync($"{_endpoint}", model.ToJsonHttpContent());
        var result = await response.Content.DeserializeFromHttpContentAsync<PlayerDto>();
        
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
        var factory = new CustomWebApplicationFactory<Program>();
        _client = factory.CreateClient();

        var model = new 
        {
            Name = "Namae",
            TeamId = 1,
            IsDead = "xd"
        };

        var response = await _client.PutAsync($"{_endpoint}", model.ToJsonHttpContent());
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task Update_TeamIdFieldIsNotValid_ReturnsBadRequest()
    {
        var factory = new CustomWebApplicationFactory<Program>();
        _client = factory.CreateClient();
        
        var model = new 
        {
            Name = "Namae",
            TeamId = "haha XD",
            IsDead = true
        };
        
        var response = await _client.PutAsync($"{_endpoint}", model.ToJsonHttpContent());
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
}