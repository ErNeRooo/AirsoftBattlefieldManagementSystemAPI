using System.Net;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Room;
using AirsoftBattlefieldManagementSystemAPI.Tests.Helpers;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Controllers.RoomController;

public class RoomControllerDeleteTests
{
    private HttpClient _client;
    private string _endpoint = "/room/id/";
    
    public RoomControllerDeleteTests()
    {
        CustomWebApplicationFactory<Program> factory = new CustomWebApplicationFactory<Program>();
        _client = factory.CreateClient();
    }
    
        
    [Theory]
    [InlineData(1)]
    public async void Delete_ValidId_ReturnsNoContent(int id)
    {
        var response = await _client.DeleteAsync($"{_endpoint}{id}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Theory]
    [InlineData("2w")]
    [InlineData("2.2")]
    [InlineData("dd")]
    public async void Delete_InvalidId_ReturnsBadRequest(string roomId)
    {
        var response = await _client.DeleteAsync($"{_endpoint}{roomId}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
    
    [Theory]
    [InlineData(76734)]
    [InlineData(0)]
    [InlineData(-414)]
    public async void Delete_NotExistingRoom_ReturnsNotFound(int roomId)
    {
        var response = await _client.DeleteAsync($"{_endpoint}{roomId}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
    
    [Theory]
    [InlineData(2, 1)]
    [InlineData(4, 2)]
    [InlineData(4, 1)]
    [InlineData(3, 1)]
    [InlineData(1, 2)]
    public async void Delete_ForNonAdminPlayer_ReturnsForbidden(int senderPlayerId, int roomId)
    {
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        var response = await _client.DeleteAsync($"{_endpoint}{roomId}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}