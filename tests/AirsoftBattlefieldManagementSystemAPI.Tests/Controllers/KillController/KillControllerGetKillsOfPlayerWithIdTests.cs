using System.Net;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Kill;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Controllers.KillController;

public class KillControllerGetKillsOfPlayerWithIdTests
{
    private HttpClient _client;
    private string _endpoint = "kill/playerid/";
    
    public static IEnumerable<object[]> GetPlayerKillsTests()
    {
        var tests = new List<PlayerKillsTestData>
        {
            new()
            {
                SenderPlayerId = 1,
                QueriedPlayerId = 1,
                ExpectedKills = new List<KillDto>
                {
                    new() { KillId = 1, PlayerId = 1, Longitude = 21, Latitude = 45, Accuracy = 10, Bearing = 45, BattleId = 1 },
                    new() { KillId = 2, PlayerId = 1, Longitude = 22.862341m, Latitude = 44.4325m, Accuracy = 15, Bearing = 80, BattleId = 1 },
                    new() { KillId = 6, PlayerId = 1, Longitude = 18.22m, Latitude = 46.4m, Accuracy = 15, Bearing = 58, BattleId = 1 }
                }
            },
            new()
            {
                SenderPlayerId = 1,
                QueriedPlayerId = 2,
                ExpectedKills = new List<KillDto>
                {
                    new() { KillId = 3, PlayerId = 2, Longitude = 56.23455m, Latitude = -86.4325m, Accuracy = 1500, Bearing = 203, BattleId = 1 },
                }
            },
            new()
            {
                SenderPlayerId = 2,
                QueriedPlayerId = 1,
                ExpectedKills = new List<KillDto>
                {
                    new() { KillId = 1, PlayerId = 1, Longitude = 21, Latitude = 45, Accuracy = 10, Bearing = 45, BattleId = 1 },
                    new() { KillId = 2, PlayerId = 1, Longitude = 22.862341m, Latitude = 44.4325m, Accuracy = 15, Bearing = 80, BattleId = 1 },
                    new() { KillId = 6, PlayerId = 1, Longitude = 18.22m, Latitude = 46.4m, Accuracy = 15, Bearing = 58, BattleId = 1 }
                }
            },
            new()
            {
                SenderPlayerId = 3,
                QueriedPlayerId = 6,
                ExpectedKills = new List<KillDto>()
            },
            new()
            {
                SenderPlayerId = 4,
                QueriedPlayerId = 6,
                ExpectedKills = new List<KillDto>()
            },
            new()
            {
                SenderPlayerId = 3,
                QueriedPlayerId = 4,
                ExpectedKills = new List<KillDto>
                {
                    new() { KillId = 7, PlayerId = 4, Longitude = -124.2213m, Latitude = -34.2137m, Accuracy = 1, Bearing = 180, BattleId = 2 }
                }
            },
            new()
            {
                SenderPlayerId = 4,
                QueriedPlayerId = 3,
                ExpectedKills = new List<KillDto>
                {
                    new() { KillId = 4, PlayerId = 3, Longitude = -32.02m, Latitude = -40.4m, Accuracy = 7, Bearing = 268, BattleId = 2 },
                    new() { KillId = 5, PlayerId = 3, Longitude = -2.2m, Latitude = -0.2137m, Accuracy = 1, Bearing = 180, BattleId = 2 }
                }
            },
        };
        
        return tests.Select(x => new object[] { x });
    }
    
    public class PlayerKillsTestData
    {
        public int SenderPlayerId { get; set; }
        public int QueriedPlayerId { get; set; }
        public List<KillDto> ExpectedKills { get; set; } = new();
    }
    
    [Theory]
    [MemberData(nameof(GetPlayerKillsTests))]
    public async Task GetKillsOfPlayerWithId_ValidId_ReturnsOkAndListOfKills(PlayerKillsTestData testData)
    {
        var factory = new CustomWebApplicationFactory<Program>(testData.SenderPlayerId);
        _client = factory.CreateClient();

        var response = await _client.GetAsync($"{_endpoint}{testData.QueriedPlayerId}");
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<List<KillDto>>();
        result.ShouldNotBeNull();
        result.Count.ShouldBe(testData.ExpectedKills.Count);

        foreach (var expected in testData.ExpectedKills)
        {
            var actual = result.FirstOrDefault(x => x.KillId == expected.KillId);
            actual.ShouldNotBeNull();
            actual.PlayerId.ShouldBe(expected.PlayerId);
            actual.Longitude.ShouldBe(expected.Longitude);
            actual.Latitude.ShouldBe(expected.Latitude);
            actual.Accuracy.ShouldBe(expected.Accuracy);
            actual.Bearing.ShouldBe(expected.Bearing);
            actual.BattleId.ShouldBe(expected.BattleId);
        }
    }

    
    [Theory]
    [InlineData(0)]
    [InlineData(234641)]
    [InlineData(-1)]
    public async void GetKillsOfPlayerWithId_PlayerDoesNotExist_ReturnsNotFound(int id)
    {
        var factory = new CustomWebApplicationFactory<Program>();
        _client = factory.CreateClient();
        
        var response = await _client.GetAsync($"{_endpoint}{id}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
    
    [Theory]
    [InlineData(1, 3)]
    [InlineData(1, 4)]
    [InlineData(1, 5)]
    [InlineData(1, 6)]
    [InlineData(4, 2)]
    [InlineData(3, 1)]
    [InlineData(6, 1)]
    [InlineData(5, 1)]
    [InlineData(5, 2)]
    [InlineData(5, 3)]
    public async void GetKillsOfPlayerWithId_PlayerIsInDifferentRoom_ReturnsForbidden(int senderPlayerId, int playerId)
    {
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        var response = await _client.GetAsync($"{_endpoint}{playerId}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}