using System.Net;
using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Player;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Room;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Team;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Tests.Helpers;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Controllers.RoomController;

public class RoomControllerLeaveTests
{
    private HttpClient _client;
    private string _endpoint = "/room/leave";
    
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public async void Leave_Valid_ReturnsOkAndUpdatesPlayer(int senderPlayerId)
    {
        // Arrange
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BattleManagementSystemDbContext>();
        
        // Act
        var response = await _client.PostAsync($"{_endpoint}", null);
        
        // Assert
        Player? resultPlayer = dbContext.Player.FirstOrDefault(player => player.PlayerId == senderPlayerId);
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        resultPlayer.ShouldNotBeNull();
        resultPlayer.TeamId.ShouldBe(null);
        resultPlayer.RoomId.ShouldBe(null);
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(9)]
    [InlineData(10)]
    public async Task Leave_Valid_ClearsPlayerMapPingsAndOrders(int senderPlayerId)
    {
        // Arrange
        CustomWebApplicationFactory<Program> factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BattleManagementSystemDbContext>();
        
        // Act
        var response = await _client.PostAsync($"{_endpoint}", null);
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        dbContext.MapPing.Where(ping => ping.PlayerId == senderPlayerId).ShouldBeEmpty();
        dbContext.Order.Where(order => order.PlayerId == senderPlayerId).ShouldBeEmpty();
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    public async void Leave_PlayerIsRoomAdmin_ReturnsOkAndUpdatesRoomAdminPlayerId(int senderPlayerId)
    {
        // Arrange
        CustomWebApplicationFactory<Program> factory = new(senderPlayerId);
        _client = factory.CreateClient();

        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BattleManagementSystemDbContext>();
        
        int? leftRoomId = dbContext.Player.First(p => p.PlayerId == senderPlayerId).RoomId;
        
        // Act
        var response = await _client.PostAsync($"{_endpoint}", null);
        
        var responseFromGet = await _client.GetAsync($"room/id/{leftRoomId}");
        var room = await responseFromGet.Content.DeserializeFromHttpContentAsync<RoomWithRelatedEntitiesDto>();
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        room.AdminPlayer.ShouldBeNull();
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(9)]
    public async void Leave_PlayerIsTeamOfficer_ReturnsOkAndUpdatesTeamOfficerId(int senderPlayerId)
    {
        // Arrange
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BattleManagementSystemDbContext>();
        
        int? leftTeamId = dbContext.Player.First(p => p.PlayerId == senderPlayerId).TeamId;

        // Act
        var response = await _client.PostAsync($"{_endpoint}", null);
        
        // Assert
        var team = dbContext.Team.FirstOrDefault(t => t.TeamId == leftTeamId);
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        team.ShouldNotBeNull();
        team.OfficerPlayerId.ShouldBeNull();
    }
}