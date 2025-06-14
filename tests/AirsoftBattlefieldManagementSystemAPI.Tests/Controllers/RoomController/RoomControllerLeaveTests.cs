using System.Net;
using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Controllers.RoomController;

public class RoomControllerLeaveTests
{
    private HttpClient _client;
    private string _endpoint = "/room/leave";
    
    [Theory]
    [InlineData(1, 1)]
    [InlineData(3, 2)]
    [InlineData(5, null)]
    public async void Leave_ValidId_ReturnsOkAndUpdatesTeam(int senderPlayerId, int? roomId)
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