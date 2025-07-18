using System.Net;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Room;
using AirsoftBattlefieldManagementSystemAPI.Tests.Helpers;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Controllers.RoomController;

public class RoomControllerUpdateTests
{
    private HttpClient _client;
    private string _endpoint = "room";
    
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
        var factory = new CustomWebApplicationFactory<Program>();
        _client = factory.CreateClient();
        
        var model = new PutRoomDto
        {
            JoinCode = joinCode,
            MaxPlayers = maxPlayers,
            Password = password,
            AdminPlayerId = adminPlayerId
        };
        
        var responseFromGet = await _client.GetAsync($"room/id/{1}");
        var resultFromGet = await responseFromGet.Content.DeserializeFromHttpContentAsync<RoomWithRelatedEntitiesDto>();
        
        responseFromGet.StatusCode.ShouldBe(HttpStatusCode.OK);
        
        var response = await _client.PutAsync($"{_endpoint}", model.ToJsonHttpContent());
        var result = await response.Content.DeserializeFromHttpContentAsync<RoomDto>();
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        result.ShouldNotBeNull();
        result.RoomId.ShouldBe(1);
        result.JoinCode.ShouldBe(joinCode ?? resultFromGet.JoinCode);
        result.MaxPlayers.ShouldBe(maxPlayers ?? resultFromGet.MaxPlayers);
        result.AdminPlayerId.ShouldBe(adminPlayerId ?? resultFromGet.AdminPlayer.PlayerId);
    }
    
    [Theory]
    [InlineData(7, 3)]
    [InlineData(8, 3)]
    public async void Update_ForNonAdminPlayerWhenThereIsNoRoomAdmin_ReturnsOkAndRoomDto(int senderPlayerId, int roomId)
    {
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        var model = new PutRoomDto
        {
            AdminPlayerId = senderPlayerId
        };
        
        var responseFromGet = await _client.GetAsync($"room/id/{roomId}");
        var resultFromGet = await responseFromGet.Content.DeserializeFromHttpContentAsync<RoomDto>();
        
        responseFromGet.StatusCode.ShouldBe(HttpStatusCode.OK);
        
        var response = await _client.PutAsync($"{_endpoint}", model.ToJsonHttpContent());
        var result = await response.Content.DeserializeFromHttpContentAsync<RoomDto>();
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        result.ShouldNotBeNull();
        result.RoomId.ShouldBe(roomId);
        result.JoinCode.ShouldBe(resultFromGet.JoinCode);
        result.MaxPlayers.ShouldBe(resultFromGet.MaxPlayers);
        result.AdminPlayerId.ShouldBe(senderPlayerId);
    }
    
    [Theory]
    [InlineData(2)]
    [InlineData(4)]
    [InlineData(6)]
    public async void Update_ForNonAdminPlayer_ReturnsForbidden(int senderPlayerId)
    {
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        var model = new PutRoomDto();
        
        var response = await _client.PutAsync($"{_endpoint}", model.ToJsonHttpContent());
        
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
    
    [Theory]
    [InlineData(5)]
    public async void Update_ForPlayerWithoutRoom_ReturnsNotFound(int senderPlayerId)
    {
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        var model = new PutRoomDto();
        
        var response = await _client.PutAsync($"{_endpoint}", model.ToJsonHttpContent());
        
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}