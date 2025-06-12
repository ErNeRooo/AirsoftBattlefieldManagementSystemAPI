using System.Net;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Controllers.RoomController;

public class RoomControllerLeaveTests
{
    private HttpClient _client;
    private string _endpoint = "/room/leave";
    
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(5)]
    public async void Leave_ValidId_ReturnsOk(int senderPlayerId)
    {
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        var response = await _client.PostAsync($"{_endpoint}", null);
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }
}