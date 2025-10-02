using System.Net;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Battle;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Player;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Room;
using AirsoftBattlefieldManagementSystemAPI.Tests.Helpers;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Controllers.RoomController;

public class RoomControllerGetByJoinCodeTests
{
    private HttpClient _client;
    private string _endpoint = "room/join-code/";

    public static IEnumerable<object[]> GetTests()
    {
        var tests = new List<GetRoomByJoinCodeTestData>
        {
            new(
                SenderPlayerId: 1,
                RoomId: 1,
                JoinCode: "123456",
                MaxPlayers: 100,
                AdminPlayer: new PlayerDto
                {
                    PlayerId = 1,
                    Name = "Chisato",
                    IsDead = false,
                    RoomId = 1,
                    TeamId = 1,
                },
                TeamsCount: 2,
                PlayersCount: 2,
                Battle: new BattleDto
                {
                    BattleId = 1,
                    IsActive = false,
                    Name = "Kursk",
                    RoomId = 1,
                }),
            
            new(
                SenderPlayerId: 5,
                RoomId: 2,
                JoinCode: "213700",
                MaxPlayers: 3,
                AdminPlayer: new PlayerDto
                {
                    PlayerId = 3,
                    Name = "Ruby",
                    IsDead = true,
                    RoomId = 2,
                    TeamId = 3
                },
                TeamsCount: 3,
                PlayersCount: 7,
                Battle: new BattleDto
                {
                    BattleId = 2,
                    IsActive = true,
                    Name = "Rhine",
                    RoomId = 2,
                }),
        };
        
        return tests.Select(x => new object[] { x });
    }

    public record GetRoomByJoinCodeTestData(
        int SenderPlayerId, 
        int RoomId, 
        string JoinCode, 
        int MaxPlayers, 
        PlayerDto? AdminPlayer,
        int TeamsCount,
        int PlayersCount,
        BattleDto? Battle
    );
    
    [Theory]
    [MemberData(nameof(GetTests))]
    public async void GetByJoinCode_ForValidModel_ReturnsOKAndRoomWithRelatedEntitiesDto(GetRoomByJoinCodeTestData testData)
    {
        var factory = new CustomWebApplicationFactory<Program>(testData.SenderPlayerId);
        _client = factory.CreateClient();
        
        var response = await _client.GetAsync($"{_endpoint}{testData.JoinCode}");
        var result = await response.Content.DeserializeFromHttpContentAsync<RoomWithRelatedEntitiesDto>();
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        result.ShouldNotBeNull();
        result.RoomId.ShouldBe(testData.RoomId);
        result.JoinCode.ShouldBe(testData.JoinCode);
        result.MaxPlayers.ShouldBe(testData.MaxPlayers);
        result.Players.Count.ShouldBe(testData.PlayersCount);
        result.Teams.Count.ShouldBe(testData.TeamsCount);
        
        result.Battle.BattleId.ShouldBe(testData.Battle.BattleId);
        result.Battle.IsActive.ShouldBe(testData.Battle.IsActive);
        result.Battle.Name.ShouldBe(testData.Battle.Name);
        result.Battle.RoomId.ShouldBe(testData.Battle.RoomId);
        
        result.AdminPlayer.PlayerId.ShouldBe(testData.AdminPlayer.PlayerId);
        result.AdminPlayer.Name.ShouldBe(testData.AdminPlayer.Name);
        result.AdminPlayer.IsDead.ShouldBe(testData.AdminPlayer.IsDead);
        result.AdminPlayer.TeamId.ShouldBe(testData.AdminPlayer.TeamId);
        result.AdminPlayer.RoomId.ShouldBe(testData.AdminPlayer.RoomId);
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