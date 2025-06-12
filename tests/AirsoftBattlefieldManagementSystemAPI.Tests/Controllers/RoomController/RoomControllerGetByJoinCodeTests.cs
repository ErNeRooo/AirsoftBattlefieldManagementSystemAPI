using System.Net;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Room;
using AirsoftBattlefieldManagementSystemAPI.Tests.Helpers;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Controllers.RoomController;

public class RoomControllerGetByJoinCodeTests
{
    private HttpClient _client;
    private string _endpoint = "room/join-code/";

    [Theory]
    [InlineData(1, 1, "123456", 100, 1)]
    [InlineData(1, 2, "213700", 2, 3)]
    [InlineData(5, 2, "213700", 2, 3)]
    public async void GetByJoinCode_ForValidModel_ReturnsOKAndRoomDto(int senderPlayerId, int roomId, string joinCode, int maxPlayers, int adminPlayerId)
    {
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        var response = await _client.GetAsync($"{_endpoint}{joinCode}");
        var result = await response.Content.DeserializeFromHttpContentAsync<RoomDto>();
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        result.ShouldNotBeNull();
        result.RoomId.ShouldBe(roomId);
        result.JoinCode.ShouldBe(joinCode);
        result.MaxPlayers.ShouldBe(maxPlayers);
        result.AdminPlayerId.ShouldBe(adminPlayerId);
    }
    
    [Theory]
    [InlineData("873518")]
    [InlineData("d3w2da")]
    [InlineData("@*%0)o")]
    public async void GetByJoinCode_ForRoomThatDoesntExist_ReturnsNotFound(string joinCode)
    {
        var factory = new CustomWebApplicationFactory<Program>();
        _client = factory.CreateClient();
        
        var response = await _client.GetAsync($"{_endpoint}{joinCode}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
    
    [Theory]
    [InlineData("12")]
    [InlineData("gjtfd")]
    [InlineData("}5..24w")]
    public async void GetByJoinCode_ForInvalidJoinCode_ReturnsBadRequest(string joinCode)
    {
        var factory = new CustomWebApplicationFactory<Program>();
        _client = factory.CreateClient();
        
        var response = await _client.GetAsync($"{_endpoint}{joinCode}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
}