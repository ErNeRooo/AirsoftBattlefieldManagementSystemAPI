using System.Net;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Death;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Controllers.DeathController;

public class DeathControllerGetDeathsOfPlayerWithIdTests
{
    private HttpClient _client;
    private string _endpoint = "death/playerid/";
    
    public static IEnumerable<object[]> GetPlayerDeathsTests()
    {
        var tests = new List<PlayerDeathsTestData>
        {
            new()
            {
                SenderPlayerId = 1,
                QueriedPlayerId = 1,
                ExpectedDeaths = new List<DeathDto>
                {
                    new() { DeathId = 1, PlayerId = 1, Longitude = 21, Latitude = 45, Accuracy = 10, Bearing = 45, RoomId = 1 },
                    new() { DeathId = 2, PlayerId = 1, Longitude = 22.862341m, Latitude = 44.4325m, Accuracy = 15, Bearing = 80, RoomId = 1 },
                    new() { DeathId = 6, PlayerId = 1, Longitude = 18.22m, Latitude = 46.4m, Accuracy = 15, Bearing = 58, RoomId = 1 }
                }
            },
            new()
            {
                SenderPlayerId = 1,
                QueriedPlayerId = 2,
                ExpectedDeaths = new List<DeathDto>
                {
                    new() { DeathId = 3, PlayerId = 2, Longitude = 56.23455m, Latitude = -86.4325m, Accuracy = 1500, Bearing = 203, RoomId = 1 },
                }
            },
            new()
            {
                SenderPlayerId = 2,
                QueriedPlayerId = 1,
                ExpectedDeaths = new List<DeathDto>
                {
                    new() { DeathId = 1, PlayerId = 1, Longitude = 21, Latitude = 45, Accuracy = 10, Bearing = 45, RoomId = 1 },
                    new() { DeathId = 2, PlayerId = 1, Longitude = 22.862341m, Latitude = 44.4325m, Accuracy = 15, Bearing = 80, RoomId = 1 },
                    new() { DeathId = 6, PlayerId = 1, Longitude = 18.22m, Latitude = 46.4m, Accuracy = 15, Bearing = 58, RoomId = 1 }
                }
            },
            new()
            {
                SenderPlayerId = 3,
                QueriedPlayerId = 6,
                ExpectedDeaths = new List<DeathDto>()
            },
            new()
            {
                SenderPlayerId = 4,
                QueriedPlayerId = 6,
                ExpectedDeaths = new List<DeathDto>()
            },
            new()
            {
                SenderPlayerId = 3,
                QueriedPlayerId = 4,
                ExpectedDeaths = new List<DeathDto>
                {
                    new() { DeathId = 7, PlayerId = 4, Longitude = -124.2213m, Latitude = -34.2137m, Accuracy = 1, Bearing = 180, RoomId = 2 }
                }
            },
            new()
            {
                SenderPlayerId = 4,
                QueriedPlayerId = 3,
                ExpectedDeaths = new List<DeathDto>
                {
                    new() { DeathId = 4, PlayerId = 3, Longitude = -32.02m, Latitude = -40.4m, Accuracy = 7, Bearing = 268, RoomId = 2 },
                    new() { DeathId = 5, PlayerId = 3, Longitude = -2.2m, Latitude = -0.2137m, Accuracy = 1, Bearing = 180, RoomId = 2 }
                }
            },
        };
        
        return tests.Select(x => new object[] { x });
    }
    
    public class PlayerDeathsTestData
    {
        public int SenderPlayerId { get; set; }
        public int QueriedPlayerId { get; set; }
        public List<DeathDto> ExpectedDeaths { get; set; } = new();
    }
    
    [Theory]
    [MemberData(nameof(GetPlayerDeathsTests))]
    public async Task GetDeathsOfPlayerWithId_ValidId_ReturnsOkAndListOfDeaths(PlayerDeathsTestData testData)
    {
        var factory = new CustomWebApplicationFactory<Program>(testData.SenderPlayerId);
        _client = factory.CreateClient();

        var response = await _client.GetAsync($"{_endpoint}{testData.QueriedPlayerId}");
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<List<DeathDto>>();
        result.ShouldNotBeNull();
        result.Count.ShouldBe(testData.ExpectedDeaths.Count);

        foreach (var expected in testData.ExpectedDeaths)
        {
            var actual = result.FirstOrDefault(x => x.DeathId == expected.DeathId);
            actual.ShouldNotBeNull();
            actual.PlayerId.ShouldBe(expected.PlayerId);
            actual.Longitude.ShouldBe(expected.Longitude);
            actual.Latitude.ShouldBe(expected.Latitude);
            actual.Accuracy.ShouldBe(expected.Accuracy);
            actual.Bearing.ShouldBe(expected.Bearing);
            actual.RoomId.ShouldBe(expected.RoomId);
        }
    }

    
    [Theory]
    [InlineData(0)]
    [InlineData(234641)]
    [InlineData(-1)]
    public async void GetDeathsOfPlayerWithId_PlayerDoesNotExist_ReturnsNotFound(int id)
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
    public async void GetDeathsOfPlayerWithId_PlayerIsInDifferentRoom_ReturnsForbidden(int senderPlayerId, int playerId)
    {
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        var response = await _client.GetAsync($"{_endpoint}{playerId}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}