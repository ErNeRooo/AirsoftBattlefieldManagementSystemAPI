using System.Net;
using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Tests.Helpers;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Player;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Team;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Controllers.PlayerController;

public class PlayerControllerUpdateTests
{
    private HttpClient _client;
    private readonly string _endpoint = "player/playerid/";

    [Theory]
    [InlineData(4, "Chisato", true, 3)]
    [InlineData(4, "Takina", false, 4)]
    public async Task Update_Self_ReturnsOkAndPlayerDto(int senderPlayerId, string name, bool isDead, int teamId)
    {
        // Arrange
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        var model = new PutPlayerDto
        {
            Name = name,
            TeamId = teamId,
            IsDead = isDead
        };
        
        // Act
        var response = await _client.PutAsync($"{_endpoint}{senderPlayerId}", model.ToJsonHttpContent());
        PlayerDto? resultPlayer = await response.Content.DeserializeFromHttpContentAsync<PlayerDto>();
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        resultPlayer.ShouldNotBeNull();
        resultPlayer.Name.ShouldBe(name);
        resultPlayer.TeamId.ShouldBe(teamId);
        resultPlayer.IsDead.ShouldBe(isDead);
    }
    
    [Theory]
    [InlineData(1,2,1)]
    [InlineData(3,10,3)]
    public async Task Update_TeamOfOtherPlayerAsAdmin_ReturnsOkAndPlayerDto(int senderPlayerId, int targetPlayerId, int teamId)
    {
        // Arrange
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BattleManagementSystemDbContext>();
        
        var model = new PutPlayerDto
        {
            TeamId = teamId,
        };
        
        Player? playerBeforeUpdate = dbContext.Player.FirstOrDefault(player => player.PlayerId == targetPlayerId);
        
        // Act
        var response = await _client.PutAsync($"{_endpoint}{targetPlayerId}", model.ToJsonHttpContent());
        PlayerDto? resultPlayer = await response.Content.DeserializeFromHttpContentAsync<PlayerDto>();
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        resultPlayer.ShouldNotBeNull();
        resultPlayer.Name.ShouldBe(playerBeforeUpdate!.Name);
        resultPlayer.TeamId.ShouldBe(teamId);
        resultPlayer.IsDead.ShouldBe(playerBeforeUpdate!.IsDead);
    }
    
    [Theory]
    [InlineData(2,1,2)]
    [InlineData(9,4,5)]
    [InlineData(10,4,5)]
    public async Task Update_TeamOfOtherPlayerAsNotAdmin_ReturnsForbidden(int senderPlayerId, int targetPlayerId, int teamId)
    {
        // Arrange
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        var model = new PutPlayerDto
        {
            TeamId = teamId,
        };
        
        // Act
        var response = await _client.PutAsync($"{_endpoint}{targetPlayerId}", model.ToJsonHttpContent());
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
    
    [Theory]
    [InlineData(2,1)]
    [InlineData(1,2)]
    [InlineData(3,4)]
    [InlineData(6,3)]
    public async Task Update_WhenPlayerIsTeamOfficerAndSwitchesTeam_ReturnsOkAndUpdatesTeam(int senderPlayerId, int teamId)
    {
        // Arrange
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BattleManagementSystemDbContext>();
        
        var model = new PutPlayerDto { TeamId = teamId };
        
        int? previousTeamId = dbContext.Player.First(player => player.PlayerId == senderPlayerId).TeamId;
        
        // Act
        var response = await _client.PutAsync($"{_endpoint}{senderPlayerId}", model.ToJsonHttpContent());
        
        // Assert
        Team resultTeam = dbContext.Team.First(team => team.TeamId == previousTeamId);
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        resultTeam.OfficerPlayerId.ShouldBeNull();
    }
    
    [Theory]
    [InlineData(1, 2)]
    [InlineData(2, 1)]
    [InlineData(3, 4)]
    [InlineData(4, 6)]
    [InlineData(9, 3)]
    [InlineData(10, 4)]
    public async Task Update_WhenPlayerSwitchesTeam_ClearsPlayerMapPingsAndOrders(int senderPlayerId, int targetTeamId)
    {
        // Arrange
        CustomWebApplicationFactory<Program> factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BattleManagementSystemDbContext>();
        
        var model = new PutPlayerDto { TeamId = targetTeamId };
        
        // Act
        var response = await _client.PutAsync($"{_endpoint}{senderPlayerId}", model.ToJsonHttpContent());
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        dbContext.MapPing.Where(ping => ping.PlayerId == senderPlayerId).ShouldBeEmpty();
        dbContext.Order.Where(order => order.PlayerId == senderPlayerId).ShouldBeEmpty();
    }
    
    [Theory]
    [InlineData(1, 3)]
    [InlineData(4, 1)]
    public async Task Update_GivenTeamIdOfForeignRoom_ReturnsForbidden(int senderPlayerId, int teamId)
    {
        // Arrange
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        var model = new PutPlayerDto { TeamId = teamId };
        
        // Act
        var response = await _client.PutAsync($"{_endpoint}{senderPlayerId}", model.ToJsonHttpContent());
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
    
    [Theory]
    [InlineData(1, null, null, null)]
    [InlineData(1, "Takina", null, null)]
    [InlineData(1, null, null, 2)]
    [InlineData(1, null, true, null)]
    [InlineData(1, "", true, null)]
    public async Task Update_GivenOnlySpecificField_ReturnsOkAndPlayerDto(int senderPlayerId, string? name, bool? isDead, int? teamId)
    {
        // Arrange
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        var model = new PutPlayerDto
        {
            Name = name,
            TeamId = teamId,
            IsDead = isDead
        };
        
        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BattleManagementSystemDbContext>();
        
        Player? playerBeforeUpdate = dbContext.Player.FirstOrDefault(player => player.PlayerId == senderPlayerId);
        
        // Act
        var response = await _client.PutAsync($"{_endpoint}{senderPlayerId}", model.ToJsonHttpContent());
        PlayerDto? resultPlayer = await response.Content.DeserializeFromHttpContentAsync<PlayerDto>();
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        resultPlayer.ShouldNotBeNull();
        resultPlayer.PlayerId.ShouldBe(1);
        resultPlayer.RoomId.ShouldBe(1);
        resultPlayer.Name.ShouldBe(string.IsNullOrEmpty(name) ? playerBeforeUpdate?.Name : name);
        resultPlayer.TeamId.ShouldBe(teamId ?? 1);
        resultPlayer.IsDead.ShouldBe(isDead ?? false);
    }
}