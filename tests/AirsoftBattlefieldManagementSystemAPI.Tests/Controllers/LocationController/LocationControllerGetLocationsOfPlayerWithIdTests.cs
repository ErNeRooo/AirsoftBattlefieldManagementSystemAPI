using System.Net;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Location;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Controllers.LocationController;

public class LocationControllerGetLocationsOfPlayerWithIdTests
{
    private HttpClient _client;
    private string _endpoint = "location/playerid/";
    
    public static IEnumerable<object[]> GetPlayerLocationsTests()
    {
        var tests = new List<PlayerLocationsTestData>
        {
            new()
            {
                SenderPlayerId = 1,
                QueriedPlayerId = 1,
                ExpectedLocations = new List<LocationDto>
                {
                    new() { LocationId = 1, PlayerId = 1, Longitude = 21, Latitude = 45, Accuracy = 10, Bearing = 45, RoomId = 1 },
                    new() { LocationId = 2, PlayerId = 1, Longitude = 22.67m, Latitude = 46, Accuracy = 5, Bearing = 58, RoomId = 1 },
                    new() { LocationId = 3, PlayerId = 1, Longitude = 18.22m, Latitude = 46.4m, Accuracy = 15, Bearing = 58, RoomId = 1 }
                }
            },
            new()
            {
                SenderPlayerId = 2,
                QueriedPlayerId = 2,
                ExpectedLocations = new List<LocationDto>
                {
                    new() { LocationId = 4, PlayerId = 2, Longitude = 32.02m, Latitude = 40.4m, Accuracy = 7, Bearing = 268, RoomId = 1 }
                }
            },
            new()
            {
                SenderPlayerId = 3,
                QueriedPlayerId = 3,
                ExpectedLocations = new List<LocationDto>
                {
                    new() { LocationId = 5, PlayerId = 3, Longitude = -32.02m, Latitude = -40.4m, Accuracy = 7, Bearing = 268, RoomId = 2 },
                    new() { LocationId = 6, PlayerId = 3, Longitude = -35.02m, Latitude = -39.42m, Accuracy = 1, Bearing = 230, RoomId = 2 }
                }
            },
            new()
            {
                SenderPlayerId = 4,
                QueriedPlayerId = 3,
                ExpectedLocations = new List<LocationDto>
                {
                    new() { LocationId = 5, PlayerId = 3, Longitude = -32.02m, Latitude = -40.4m, Accuracy = 7, Bearing = 268, RoomId = 2 },
                    new() { LocationId = 6, PlayerId = 3, Longitude = -35.02m, Latitude = -39.42m, Accuracy = 1, Bearing = 230, RoomId = 2 }
                }
            }
        };
    
        return tests.Select(x => new object[] { x });
    }
    
    public class PlayerLocationsTestData
    {
        public int SenderPlayerId { get; set; }
        public int QueriedPlayerId { get; set; }
        public List<LocationDto> ExpectedLocations { get; set; } = new();
    }
    
    [Theory]
    [MemberData(nameof(GetPlayerLocationsTests))]
    public async Task GetLocationsOfPlayerWithId_ValidId_ReturnsOkAndListOfLocations(PlayerLocationsTestData testData)
    {
        var factory = new CustomWebApplicationFactory<Program>(testData.SenderPlayerId);
        _client = factory.CreateClient();

        var response = await _client.GetAsync($"{_endpoint}{testData.QueriedPlayerId}");
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<List<LocationDto>>();
        result.ShouldNotBeNull();
        result.Count.ShouldBe(testData.ExpectedLocations.Count);

        foreach (var expected in testData.ExpectedLocations)
        {
            var actual = result.FirstOrDefault(x => x.LocationId == expected.LocationId);
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
    public async void GetLocationsOfPlayerWithId_PlayerDoesNotExist_ReturnsNotFound(int id)
    {
        CustomWebApplicationFactory<Program> factory = new CustomWebApplicationFactory<Program>();
        _client = factory.CreateClient();
        
        var response = await _client.GetAsync($"{_endpoint}{id}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
    
    [Theory]
    [InlineData(1, 2)]
    [InlineData(1, 3)]
    [InlineData(1, 4)]
    [InlineData(1, 5)]
    [InlineData(1, 6)]
    [InlineData(4, 6)]
    [InlineData(4, 2)]
    [InlineData(5, 1)]
    [InlineData(5, 2)]
    [InlineData(5, 3)]
    public async void GetLocationsOfPlayerWithId_PlayerIsInDifferentTeam_ReturnsForbidden(int senderPlayerId, int playerId)
    {
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        var response = await _client.GetAsync($"{_endpoint}{playerId}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}