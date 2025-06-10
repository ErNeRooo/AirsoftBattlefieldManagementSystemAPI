using System.Net;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Room;
using AirsoftBattlefieldManagementSystemAPI.Tests.Helpers;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Controllers.RoomController;

public class RoomControllerUpdateTests
{
    private HttpClient _client;
    private string _endpoint = "room/id/";

    public RoomControllerUpdateTests()
    {
        CustomWebApplicationFactory<Program> factory = new CustomWebApplicationFactory<Program>();
        _client = factory.CreateClient();
    }
    
    [Theory]
    [InlineData("325543", 2, "creeper", 2)]
    [InlineData("aaaaaa", 2, "aw maaan", 2)]
    [InlineData(null, null, null, null)]
    [InlineData("76w231", null, null, null)]
    [InlineData(null, 5, null, null)]
    [InlineData(null, null, "haha", null)]
    [InlineData(null, null, null, 2)]
    public async void Update_ValidModel_ReturnsOkAndRoomDto(string? joinCode, int? maxPlayers, string? password, int? adminPlayerId)
    {
        var model = new PutRoomDto
        {
            JoinCode = joinCode,
            MaxPlayers = maxPlayers,
            Password = password,
            AdminPlayerId = adminPlayerId
        };
        
        var responseFromGet = await _client.GetAsync($"room/id/{1}");
        var resultFromGet = await responseFromGet.Content.DeserializeFromHttpContentAsync<RoomDto>();
        
        responseFromGet.StatusCode.ShouldBe(HttpStatusCode.OK);
        
        var response = await _client.PutAsync($"{_endpoint}{1}", model.ToJsonHttpContent());
        var result = await response.Content.DeserializeFromHttpContentAsync<RoomDto>();
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        result.ShouldNotBeNull();
        result.RoomId.ShouldBe(1);
        result.JoinCode.ShouldBe(joinCode ?? resultFromGet.JoinCode);
        result.MaxPlayers.ShouldBe(maxPlayers ?? resultFromGet.MaxPlayers);
        result.AdminPlayerId.ShouldBe(adminPlayerId ?? resultFromGet.AdminPlayerId);
    }

    [Theory]
    [InlineData("2w")]
    [InlineData("2.2")]
    [InlineData("dd")]
    public async void Update_InvalidId_ReturnsBadRequest(string roomId)
    {
        var model = new PutRoomDto();
        
        var response = await _client.PutAsync($"{_endpoint}{roomId}", model.ToJsonHttpContent());
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
    
    [Theory]
    [InlineData(76734)]
    [InlineData(0)]
    [InlineData(-414)]
    public async void Update_NotExistingRoom_ReturnsNotFound(int roomId)
    {
        var model = new PutRoomDto();
        
        var response = await _client.PutAsync($"{_endpoint}{roomId}", model.ToJsonHttpContent());
        
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
    
    [Theory]
    [InlineData(2, 1)]
    [InlineData(4, 2)]
    [InlineData(4, 1)]
    [InlineData(3, 1)]
    [InlineData(1, 2)]
    public async void Update_ForNonAdminPlayer_ReturnsForbidden(int senderPlayerId, int roomId)
    {
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        var model = new PutRoomDto();
        
        var response = await _client.PutAsync($"{_endpoint}{roomId}", model.ToJsonHttpContent());
        
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}