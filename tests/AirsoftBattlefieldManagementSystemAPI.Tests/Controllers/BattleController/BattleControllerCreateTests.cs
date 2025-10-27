using System.Net;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Battle;
using AirsoftBattlefieldManagementSystemAPI.Tests.Helpers;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Controllers.BattleController;

public class BattleControllerCreateTests
{
    private HttpClient _client;
    private string _endpoint = "battle";
        
    [Theory]
    [InlineData(14, 5, "Monte Casino")]
    public async void Create_ValidModel_ReturnsCreatedAndBattleDto(int senderPlayerId, int roomId, string name)
    {
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();

        var model = new PostBattleDto()
        {
            Name = name,
            RoomId = roomId
        };
        
        var response = await _client.PostAsync($"{_endpoint}", model.ToJsonHttpContent());
        var result = await response.Content.DeserializeFromHttpContentAsync<BattleDto>();
        
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        result.BattleId.ShouldNotBe(0);
        result.Name.ShouldBe(name);
        result.RoomId.ShouldBe(roomId);
    }
    
    [Theory]
    [InlineData(1, 2)]
    [InlineData(2, 1)]
    [InlineData(4, 2)]
    [InlineData(6, 2)]
    [InlineData(6, 1)]
    [InlineData(5, 1)]
    [InlineData(5, 2)]
    public async void Create_PlayerIsNotRoomAdmin_ReturnsForbidden(int senderPlayerId, int roomId)
    {
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();

        var model = new PostBattleDto
        {
            Name = "Nice Battle",
            RoomId = roomId
        };
        
        var response = await _client.PostAsync($"{_endpoint}", model.ToJsonHttpContent());

        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
    
    [Theory]
    [InlineData(1, 1)]
    [InlineData(3, 2)]
    public async void Create_RoomAlreadyHasBattle_ReturnsBadRequest(int senderPlayerId, int roomId)
    {
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();

        var model = new PostBattleDto
        {
            Name = "Nice Battle",
            RoomId = roomId
        };
        
        var response = await _client.PostAsync($"{_endpoint}", model.ToJsonHttpContent());

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
}