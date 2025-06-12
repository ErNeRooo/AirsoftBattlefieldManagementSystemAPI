using System.Net;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Room;
using AirsoftBattlefieldManagementSystemAPI.Tests.Helpers;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Controllers.RoomController;

public class RoomControllerDeleteTests
{
    private HttpClient _client;
    private string _endpoint = "room";
    
    public RoomControllerDeleteTests()
    {
        CustomWebApplicationFactory<Program> factory = new CustomWebApplicationFactory<Program>();
        _client = factory.CreateClient();
    }
    
    [Fact]
    public async void Delete_ValidId_ReturnsNoContent()
    {
        var response = await _client.DeleteAsync($"{_endpoint}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
    
    [Theory]
    [InlineData(2)]
    [InlineData(4)]
    public async void Delete_ForNonAdminPlayer_ReturnsForbidden(int senderPlayerId)
    {
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        var response = await _client.DeleteAsync($"{_endpoint}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
    
    [Theory]
    [InlineData(5)]
    public async void Delete_ForPlayerWithoutRoom_ReturnsNotFound(int senderPlayerId)
    {
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        var response = await _client.DeleteAsync($"{_endpoint}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}