using System.Net;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Room;
using AirsoftBattlefieldManagementSystemAPI.Tests.Helpers;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Controllers.RoomController;

public class RoomControllerCreateTests
{
    private HttpClient _client;
    private string _endpoint = "room/";

    [Theory]
    [InlineData(5, "325543", 2, "")]
    public async void Create_ForValidModel_ReturnsCreatedAndRoomDto(int senderPlayerId, string joinCode, int maxPlayers, string password)
    {
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();

        var model = new PostRoomDto
        {
            JoinCode = joinCode,
            MaxPlayers = maxPlayers,
            Password = password
        };
        
        var response = await _client.PostAsync($"{_endpoint}", model.ToJsonHttpContent());
        var result = await response.Content.DeserializeFromHttpContentAsync<RoomDto>();
        
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        result.ShouldNotBeNull();
        result.RoomId.ShouldNotBe(0);
        result.JoinCode.ShouldBe(joinCode);
        result.MaxPlayers.ShouldBe(maxPlayers);
        result.AdminPlayerId.ShouldBe(senderPlayerId);
    }
    
    [Theory]
    [InlineData(5, 86)]
    public async void Create_JoinCodeIsMissing_ReturnsCreatedAndRoomDto(int senderPlayerId, int maxPlayers)
    {
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();

        var model = new PostRoomDto
        {
            MaxPlayers = maxPlayers,
        };
        
        var response = await _client.PostAsync($"{_endpoint}", model.ToJsonHttpContent());
        var result = await response.Content.DeserializeFromHttpContentAsync<RoomDto>();
        
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        result.ShouldNotBeNull();
        result.RoomId.ShouldNotBe(0);
        result.JoinCode.Length.ShouldBe(6);
        result.MaxPlayers.ShouldBe(maxPlayers);
        result.AdminPlayerId.ShouldBe(senderPlayerId);
    }
    
    [Theory]
    [InlineData(1, "543233", 100, "")]
    [InlineData(2, "fse2h3", 5, "yes")]
    public async void Create_ForPlayerThatIsAlreadyInRoom_ReturnsConflict(int senderPlayerId, string joinCode, int maxPlayers, string password)
    {
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();

        var model = new PostRoomDto
        {
            JoinCode = joinCode,
            MaxPlayers = maxPlayers,
            Password = password
        };
        
        var response = await _client.PostAsync($"{_endpoint}", model.ToJsonHttpContent());
        
        response.StatusCode.ShouldBe(HttpStatusCode.Conflict);
    }
    

}