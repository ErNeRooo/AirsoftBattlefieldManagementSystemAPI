using System.Net;
using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Tests.Helpers;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Player;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Team;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Controllers.TeamController;

public class TeamControllerLeaveTests
{
    private HttpClient _client;
    private readonly string _endpoint = "team/leave";
    
    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(9)]
    [InlineData(10)]
    public async Task Leave_Anyone_ReturnsOkClearsPlayerOrdersAndMapPings(int senderPlayerId)
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
        resultPlayer.TeamId.ShouldBeNull(); 
        dbContext.Order.Any(order => order.PlayerId == senderPlayerId).ShouldBeFalse();
        dbContext.MapPing.Any(mapPing => mapPing.PlayerId == senderPlayerId).ShouldBeFalse();
    }
    
    [Theory]
    [InlineData(2)]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(6)]
    public async Task Leave_WhenPlayerIsTeamOfficer_UpdatesTeam(int senderPlayerId)
    {
        // Arrange
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BattleManagementSystemDbContext>();
        
        int? previousTeamId = dbContext.Player.First(player => player.PlayerId == senderPlayerId).TeamId;
        
        // Act
        var response = await _client.PostAsync($"{_endpoint}", null);
        
        // Assert
        Team resultTeam = dbContext.Team.First(team => team.TeamId == previousTeamId);
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        resultTeam.OfficerPlayerId.ShouldBeNull();
    }
}