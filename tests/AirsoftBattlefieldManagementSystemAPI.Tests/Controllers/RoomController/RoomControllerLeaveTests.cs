using System.Net;
using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Player;
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
    public async void Leave_ValidId_ReturnsOkAndUpdatesPlayer(int senderPlayerId)
    {
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        var response = await _client.PostAsync($"{_endpoint}", null);
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        
        var responseFromGet = await _client.GetAsync("/player/me");
        var result = await responseFromGet.Content.DeserializeFromHttpContentAsync<PlayerDto>();
        
        result.TeamId.ShouldBe(null);
        result.RoomId.ShouldBe(null);
    }
    
    [Theory]
    [InlineData(1, 1)]
    [InlineData(3, 2)]
    public async void Leave_PlayerIsRoomAdmin_ReturnsOkAndUpdatesRoom(int senderPlayerId, int? roomId)
    {
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        var response = await _client.PostAsync($"{_endpoint}", null);
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        if (roomId is not null)
        {
            using var scope = factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<BattleManagementSystemDbContext>();
        
            var room = context.Room.FirstOrDefault(t => t.RoomId == roomId);
        
            room.AdminPlayerId.ShouldBeNull();
        }
    }
}