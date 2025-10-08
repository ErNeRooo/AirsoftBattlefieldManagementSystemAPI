using System.Net;
using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Player;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Tests.Helpers;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Controllers.PlayerController;

public class PlayerControllerKickFromRoomTests
{
    private HttpClient _client;
    private string _endpoint = "player/kick-from-room/playerId/";
    
    [Theory]
    [InlineData(3, 4)]
    [InlineData(1, 2)]
    [InlineData(3, 9)]
    [InlineData(3, 10)]
    public async Task KickFromRoom_ValidId_ReturnsOkAndPlayerDto(int senderPlayerId, int targetPlayerId)
    {
        // Arrange
        CustomWebApplicationFactory<Program> factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
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
        resultPlayer.RoomId.ShouldBeNull();
        resultPlayer.IsDead.ShouldBe(playerBeforeKick.IsDead);
        resultPlayer.Name.ShouldBe(playerBeforeKick.Name);
    }
    
    [Theory]
    [InlineData(3, 4)]
    [InlineData(1, 2)]
    [InlineData(3, 9)]
    [InlineData(3, 10)]
    public async Task KickFromRoom_ValidId_ClearsPlayerMapPingsAndOrders(int senderPlayerId, int targetPlayerId)
    {
        // Arrange
        CustomWebApplicationFactory<Program> factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
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
    public async Task KickFromRoom_TargetPlayerIsOfficer_UnassignTeamOfficerPlayerId(int senderPlayerId, int targetPlayerId)
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
    [InlineData(3, 3)]
    [InlineData(1, 1)]
    public async Task KickFromRoom_Self_ReturnsForbidden(int senderPlayerId, int targetPlayerId)
    {
        // Arrange
        CustomWebApplicationFactory<Program> factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        // Act
        var response = await _client.PostAsync($"{_endpoint}{targetPlayerId}", null);
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
    
    [Theory]
    [InlineData(4, 3)]
    [InlineData(4, 4)]
    public async Task KickFromRoom_SenderPlayerIsNotAdmin_ReturnsForbidden(int senderPlayerId, int targetPlayerId)
    {
        // Arrange
        CustomWebApplicationFactory<Program> factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
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
    public async Task KickFromRoom_PlayerWithGivenIdNotFound_ReturnsNotFound(string id)
    {
        // Arrange
        CustomWebApplicationFactory<Program> factory = new CustomWebApplicationFactory<Program>();
        _client = factory.CreateClient();
        
        // Act
        var response = await _client.PostAsync($"{_endpoint}{id}", null);
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}