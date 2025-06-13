using System.Net;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Room;
using AirsoftBattlefieldManagementSystemAPI.Tests.Helpers;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Controllers.RoomController;

public class RoomControllerGetByIdTests
{
    private HttpClient _client;
    private string _endpoint = "room/id/";
    
    [Theory]
    [InlineData(1, 1, "123456", 100, 1)]
    [InlineData(1, 2, "213700", 3, 3)]
    [InlineData(5, 2, "213700", 3, 3)]
    public async void GetById_ForValidModel_ReturnsOKAndRoomDto(int senderPlayerId, int roomId, string joinCode, int maxPlayers, int adminPlayerId)
    {
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        var response = await _client.GetAsync($"{_endpoint}{roomId}");
        var result = await response.Content.DeserializeFromHttpContentAsync<RoomDto>();
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        result.ShouldNotBeNull();
        result.RoomId.ShouldBe(roomId);
        result.JoinCode.ShouldBe(joinCode);
        result.MaxPlayers.ShouldBe(maxPlayers);
        result.AdminPlayerId.ShouldBe(adminPlayerId);
    }
    
    [Theory]
    [InlineData("54533")]
    [InlineData("")]
    [InlineData("0")]
    public async void GetById_ForRoomThatDoesntExist_ReturnsNotFound(string roomId)
    {
        var factory = new CustomWebApplicationFactory<Program>();
        _client = factory.CreateClient();
        
        var response = await _client.GetAsync($"{_endpoint}{roomId}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}