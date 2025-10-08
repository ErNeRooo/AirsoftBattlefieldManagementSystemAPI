using System.Net;
using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Player;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService;
using AirsoftBattlefieldManagementSystemAPI.Tests.Helpers;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Controllers.PlayerController;

public class PlayerControllerKickFromTeamTests
{
    private HttpClient _client;
    private string _endpoint = "player/kick-from-team/playerId/";
    
    [Theory]
    [InlineData(1, 2)]
    [InlineData(9, 10)]
    [InlineData(3, 4)]
    [InlineData(3, 9)]
    public async Task KickFromTeam_Valid_ReturnsOkAndPlayerDto(int senderPlayerId, int targetPlayerId)
    {
        // Arrange
        CustomWebApplicationFactory<Program> factory = new(senderPlayerId);
        _client = factory.CreateClient();
        
        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BattleManagementSystemDbContext>();
        
        Player playerBeforeKick = dbContext.Player.First(player => player.PlayerId == targetPlayerId);
        
        // Act
        var response = await _client.PostAsync($"{_endpoint}{targetPlayerId}", null);
        var resultPlayer = await response.Content.DeserializeFromHttpContentAsync<PlayerDto>();
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        resultPlayer.ShouldNotBeNull();
        resultPlayer.PlayerId.ShouldBe(playerBeforeKick.PlayerId);
        resultPlayer.TeamId.ShouldBeNull();
        resultPlayer.RoomId.ShouldBe(playerBeforeKick.RoomId);
        resultPlayer.IsDead.ShouldBe(playerBeforeKick.IsDead);
        resultPlayer.Name.ShouldBe(playerBeforeKick.Name);
    }
    
    [Theory]
    [InlineData(1, 2)]
    [InlineData(9, 10)]
    [InlineData(3, 4)]
    [InlineData(3, 9)]
    public async Task KickFromTeam_Valid_ClearsPlayerMapPingsAndOrders(int senderPlayerId, int targetPlayerId)
    {
        // Arrange
        CustomWebApplicationFactory<Program> factory = new(senderPlayerId);
        _client = factory.CreateClient();
        
        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BattleManagementSystemDbContext>();
        
        // Act
        var response = await _client.PostAsync($"{_endpoint}{targetPlayerId}", null);
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        dbContext.MapPing.Where(ping => ping.PlayerId == targetPlayerId).ShouldBeEmpty();
        dbContext.Order.Where(order => order.PlayerId == targetPlayerId).ShouldBeEmpty();
    }
    
    [Theory]
    [InlineData(1, 2)]
    [InlineData(3, 6)]
    [InlineData(3, 9)]
    public async Task KickFromTeam_TargetPlayerIsOfficer_UnassignTeamOfficerPlayerId(int senderPlayerId, int targetPlayerId)
    {
        // Arrange
        CustomWebApplicationFactory<Program> factory = new(senderPlayerId);
        _client = factory.CreateClient();
        
        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BattleManagementSystemDbContext>();
        
        int? leftTeamId = dbContext.Player.First(p => p.PlayerId == targetPlayerId).TeamId;
        
        // Act
        var response = await _client.PostAsync($"{_endpoint}{targetPlayerId}", null);
        
        // Assert
        Team? team = dbContext.Team.FirstOrDefault(t => t.TeamId == leftTeamId);
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        team.ShouldNotBeNull();
        team.OfficerPlayerId.ShouldBeNull();
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    public async Task KickFromTeam_Self_ReturnsForbidden(int senderPlayerId)
    {
        // Arrange
        CustomWebApplicationFactory<Program> factory = new(senderPlayerId);
        _client = factory.CreateClient();
        
        // Act
        var response = await _client.PostAsync($"{_endpoint}{senderPlayerId}", null);
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
    
    [Theory]
    [InlineData(4, 3)]
    [InlineData(10, 9)]
    [InlineData(11, 10)]
    public async Task KickFromTeam_SenderPlayerIsNotOfficerOrAdmin_ReturnsForbidden(int senderPlayerId, int targetPlayerId)
    {
        // Arrange
        CustomWebApplicationFactory<Program> factory = new(senderPlayerId);
        _client = factory.CreateClient();
        
        // Act
        var response = await _client.PostAsync($"{_endpoint}{targetPlayerId}", null);
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
    
    [Theory]
    [InlineData(9, 3)]
    [InlineData(9, 4)]
    [InlineData(6, 3)]
    [InlineData(6, 9)]
    [InlineData(6, 10)]
    [InlineData(2, 3)]
    [InlineData(2, 4)]
    public async Task KickFromTeam_SenderPlayerIsOfficerButInDifferentTeamThanTargetPlayer_ReturnsForbidden(int senderPlayerId, int targetPlayerId)
    {
        // Arrange
        CustomWebApplicationFactory<Program> factory = new(senderPlayerId);
        _client = factory.CreateClient();
        
        // Act
        var response = await _client.PostAsync($"{_endpoint}{targetPlayerId}", null);
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
    
    [Theory]
    [InlineData(1, 3)]
    [InlineData(1, 4)]
    [InlineData(1, 6)]
    [InlineData(1, 9)]
    [InlineData(1, 10)]
    [InlineData(3, 1)]
    [InlineData(3, 2)]
    public async Task KickFromTeam_SenderPlayerIsAdminButInRoomThanTargetPlayer_ReturnsForbidden(int senderPlayerId, int targetPlayerId)
    {
        // Arrange
        CustomWebApplicationFactory<Program> factory = new(senderPlayerId);
        _client = factory.CreateClient();
        
        // Act
        var response = await _client.PostAsync($"{_endpoint}{targetPlayerId}", null);
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
    
    [Theory]
    [InlineData("-123")]
    [InlineData("0")]
    [InlineData("")]
    [InlineData("12314")]
    public async Task KickFromTeam_PlayerWithGivenIdNotFound_ReturnsNotFound(string id)
    {
        // Arrange
        CustomWebApplicationFactory<Program> factory = new();
        _client = factory.CreateClient();
        
        // Act
        var response = await _client.PostAsync($"{_endpoint}{id}", null);
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}