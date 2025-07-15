using System.Net;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Battle;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Player;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Room;
using AirsoftBattlefieldManagementSystemAPI.Tests.Helpers;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Controllers.RoomController;

public class RoomControllerGetByIdTests
{
    private HttpClient _client;
    private string _endpoint = "room/id/";
    
    public static IEnumerable<object[]> GetTests()
    {
        var tests = new List<GetRoomByIdTestData>
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
                TeamsCount: 2,
                PlayersCount: 3,
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

    public record GetRoomByIdTestData(
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
    public async void GetById_ForValidModel_ReturnsOKAndRoomWithRelatedEntitiesDto(GetRoomByIdTestData testData)
    {
        var factory = new CustomWebApplicationFactory<Program>(testData.SenderPlayerId);
        _client = factory.CreateClient();
        
        var response = await _client.GetAsync($"{_endpoint}{testData.RoomId}");
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