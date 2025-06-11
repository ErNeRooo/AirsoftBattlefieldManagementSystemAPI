using System.Net;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Battle;
using AirsoftBattlefieldManagementSystemAPI.Tests.Helpers;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Controllers.BattleController;

public class BattleControllerUpdateTests
{
    private HttpClient _client;
    private string _endpoint = "battle/id/";
    
    public BattleControllerUpdateTests()
    {
        CustomWebApplicationFactory<Program> factory = new CustomWebApplicationFactory<Program>();
        _client = factory.CreateClient();
    }
    
    [Theory]
    [InlineData(1, 1, false, "Epic Battle", 1)]
    [InlineData(3, 2, true, "Another Epic Battle", 2)]
    public async void Update_ValidId_ReturnsOkAndBattleDto(int senderPlayerId, int battleId, bool isActive, string name, int roomId)
    {
        CustomWebApplicationFactory<Program> factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();

        var model = new PutBattleDto
        {
            Name = name,
            IsActive = isActive
        };
        
        var response = await _client.PutAsync($"{_endpoint}{battleId}", model.ToJsonHttpContent());
        var result = await response.Content.DeserializeFromHttpContentAsync<BattleDto>();
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        result.ShouldNotBeNull();
        result.BattleId.ShouldBe(battleId);
        result.Name.ShouldBe(name);
        result.IsActive.ShouldBe(isActive);
        result.RoomId.ShouldBe(roomId);
    }
    
    [Theory]
    [InlineData(1, 1, null, null)]
    [InlineData(3, 2, null, null)]
    [InlineData(1, 1, true, null)]
    [InlineData(1, 1, null, "Epic Battle")]
    [InlineData(3, 2, false, null)]
    [InlineData(3, 2, null, "Epic Battle")]
    public async void Update_NotAllFieldsSpecified_ReturnsOkAndBattleDto(int senderPlayerId, int battleId, bool? isActive, string? name)
    {
        CustomWebApplicationFactory<Program> factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();

        var model = new PutBattleDto
        {
            Name = name,
            IsActive = isActive
        };
        
        var responseFromGet = await _client.GetAsync($"battle/id/{battleId}");
        var resultFromGet = await responseFromGet.Content.DeserializeFromHttpContentAsync<BattleDto>();
        
        responseFromGet.StatusCode.ShouldBe(HttpStatusCode.OK);
        
        var response = await _client.PutAsync($"{_endpoint}{battleId}", model.ToJsonHttpContent());
        var result = await response.Content.DeserializeFromHttpContentAsync<BattleDto>();
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        result.ShouldNotBeNull();
        result.BattleId.ShouldBe(battleId);
        result.Name.ShouldBe(name ?? resultFromGet.Name);
        result.IsActive.ShouldBe(isActive ?? resultFromGet.IsActive);
        result.RoomId.ShouldBe(resultFromGet.RoomId);
    }
    
    [Theory]
    [InlineData("aa")]
    [InlineData("1.0")]
    public async void Update_NotValidId_ReturnsBadRequest(string battleId)
    {
        var model = new PutBattleDto();

        var response = await _client.PutAsync($"{_endpoint}{battleId}", model.ToJsonHttpContent());
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(11321)]
    public async void Update_NotExistingBattle_ReturnsNotFound(int battleId)
    {
        var model = new PutBattleDto();

        var response = await _client.PutAsync($"{_endpoint}{battleId}", model.ToJsonHttpContent());
        
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
    
    [Theory]
    [InlineData(2, 1)]
    [InlineData(2, 2)]
    [InlineData(4, 2)]
    [InlineData(5, 1)]
    [InlineData(5, 2)]
    [InlineData(6, 2)]
    [InlineData(6, 1)]
    public async void Update_PlayerIsNotRoomAdmin_ReturnsForbidden(int senderPlayerId, int battleId)
    {
        CustomWebApplicationFactory<Program> factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        var model = new PutBattleDto();

        var response = await _client.PutAsync($"{_endpoint}{battleId}", model.ToJsonHttpContent());
        
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}