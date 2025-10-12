using System.Net;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.MapPing;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Tests.Helpers;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Controllers.MapPingController;

public class MapPingControllerGetManyByTeamIdTests
{
    private HttpClient _client;
    private string _endpoint = "map-ping/teamId/";
        
    public class MapPingTestData
    {
        public int SenderPlayerId { get; set; }
        public int TeamId { get; set; }
        public List<MapPingDto> ExpectedMapPings { get; set; } = new();
    }
    
    public static IEnumerable<object[]> GetTests()
    {
        var tests = new List<MapPingTestData>
        {
            new()
            {
                SenderPlayerId = 1,
                TeamId = 1,
                ExpectedMapPings = new List<MapPingDto>
                {
                    new()
                    {
                        MapPingId = 1,
                        PlayerId = 1,
                        BattleId = 1,
                        LocationId = 1,
                        Longitude = 21,
                        Latitude = 45,
                        Accuracy = 10,
                        Bearing = 45,
                        Type = MapPingTypes.ENEMY
                    }
                }
            },
            new()
            {
                SenderPlayerId = 2,
                TeamId = 2,
                ExpectedMapPings = new List<MapPingDto>
                {
                    new()
                    {
                        MapPingId = 2,
                        PlayerId = 2,
                        BattleId = 1,
                        LocationId = 2,
                        Longitude = 22.67m,
                        Latitude = 46,
                        Accuracy = 5,
                        Bearing = 58,
                        Type = MapPingTypes.ENEMY
                    }
                }
            },
            new()
            {
                SenderPlayerId = 3,
                TeamId = 3,
                ExpectedMapPings = new List<MapPingDto>
                {
                    new()
                    {
                        MapPingId = 3,
                        PlayerId = 3,
                        BattleId = 2,
                        LocationId = 3,
                        Longitude = 18.22m,
                        Latitude = 46.4m,
                        Accuracy = 15,
                        Bearing = 58,
                        Type = MapPingTypes.ENEMY
                    },
                    new()
                    {
                        MapPingId = 4,
                        PlayerId = 4,
                        BattleId = 2,
                        LocationId = 4,
                        Longitude = 32.02m,
                        Latitude = 40.4m,
                        Accuracy = 7,
                        Bearing = 268,
                        Type = MapPingTypes.ENEMY
                    }
                }
            },
            new()
            {
                SenderPlayerId = 4,
                TeamId = 3,
                ExpectedMapPings = new List<MapPingDto>
                {
                    new()
                    {
                        MapPingId = 3,
                        PlayerId = 3,
                        BattleId = 2,
                        LocationId = 3,
                        Longitude = 18.22m,
                        Latitude = 46.4m,
                        Accuracy = 15,
                        Bearing = 58,
                        Type = MapPingTypes.ENEMY
                    },
                    new()
                    {
                        MapPingId = 4,
                        PlayerId = 4,
                        BattleId = 2,
                        LocationId = 4,
                        Longitude = 32.02m,
                        Latitude = 40.4m,
                        Accuracy = 7,
                        Bearing = 268,
                        Type = MapPingTypes.ENEMY
                    }
                }
            },
            new()
            {
                SenderPlayerId = 9,
                TeamId = 6,
                ExpectedMapPings = new List<MapPingDto>
                {
                    new()
                    {
                        MapPingId = 9,
                        PlayerId = 9,
                        BattleId = 2,
                        LocationId = 9,
                        Longitude = 56.23455m,
                        Latitude = -86.4325m,
                        Accuracy = 1500,
                        Bearing = 203,
                        Type = MapPingTypes.ENEMY
                    },
                    new()
                    {
                        MapPingId = 10,
                        PlayerId = 10,
                        BattleId = 2,
                        LocationId = 10,
                        Longitude = -2.2m,
                        Latitude = -0.2137m,
                        Accuracy = 1,
                        Bearing = 180,
                        Type = MapPingTypes.ENEMY
                    },
                    new()
                    {
                        MapPingId = 11,
                        PlayerId = 11,
                        BattleId = 2,
                        LocationId = 11,
                        Longitude = -124.2213m,
                        Latitude = -34.2137m,
                        Accuracy = 1,
                        Bearing = 180,
                        Type = MapPingTypes.ENEMY
                    }
                }
            },
            new()
            {
                SenderPlayerId = 10,
                TeamId = 6,
                ExpectedMapPings = new List<MapPingDto>
                {
                    new()
                    {
                        MapPingId = 9,
                        PlayerId = 9,
                        BattleId = 2,
                        LocationId = 9,
                        Longitude = 56.23455m,
                        Latitude = -86.4325m,
                        Accuracy = 1500,
                        Bearing = 203,
                        Type = MapPingTypes.ENEMY
                    },
                    new()
                    {
                        MapPingId = 10,
                        PlayerId = 10,
                        BattleId = 2,
                        LocationId = 10,
                        Longitude = -2.2m,
                        Latitude = -0.2137m,
                        Accuracy = 1,
                        Bearing = 180,
                        Type = MapPingTypes.ENEMY
                    },
                    new()
                    {
                        MapPingId = 11,
                        PlayerId = 11,
                        BattleId = 2,
                        LocationId = 11,
                        Longitude = -124.2213m,
                        Latitude = -34.2137m,
                        Accuracy = 1,
                        Bearing = 180,
                        Type = MapPingTypes.ENEMY
                    }
                }
            },
            new()
            {
                SenderPlayerId = 11,
                TeamId = 6,
                ExpectedMapPings = new List<MapPingDto>
                {
                    new()
                    {
                        MapPingId = 9,
                        PlayerId = 9,
                        BattleId = 2,
                        LocationId = 9,
                        Longitude = 56.23455m,
                        Latitude = -86.4325m,
                        Accuracy = 1500,
                        Bearing = 203,
                        Type = MapPingTypes.ENEMY
                    },
                    new()
                    {
                        MapPingId = 10,
                        PlayerId = 10,
                        BattleId = 2,
                        LocationId = 10,
                        Longitude = -2.2m,
                        Latitude = -0.2137m,
                        Accuracy = 1,
                        Bearing = 180,
                        Type = MapPingTypes.ENEMY
                    },
                    new()
                    {
                        MapPingId = 11,
                        PlayerId = 11,
                        BattleId = 2,
                        LocationId = 11,
                        Longitude = -124.2213m,
                        Latitude = -34.2137m,
                        Accuracy = 1,
                        Bearing = 180,
                        Type = MapPingTypes.ENEMY
                    }
                }
            },
        };
        
        return tests.Select(x => new object[] { x });
    }

    
    [Theory]
    [MemberData(nameof(GetTests))]
    public async void GetManyByTeamId_Valid_ReturnsOkAndListOfMapPingDtos(MapPingTestData testData)
    {
        var factory = new CustomWebApplicationFactory<Program>(testData.SenderPlayerId);
        _client = factory.CreateClient();
        
        var response = await _client.GetAsync($"{_endpoint}{testData.TeamId}");
        var result = await HttpContentJsonExtensions.ReadFromJsonAsync<List<MapPingDto>>(response.Content);
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        result.ShouldNotBeNull();

        for (int i = 0; i < testData.ExpectedMapPings.Count; i++)
        {
            MapPingDto expected = testData.ExpectedMapPings[i];
            MapPingDto actual = result[i];
            
            actual.MapPingId.ShouldBe(expected.MapPingId);
            actual.PlayerId.ShouldBe(expected.PlayerId);
            actual.BattleId.ShouldBe(expected.BattleId);
            actual.Longitude.ShouldBe(expected.Longitude);
            actual.Latitude.ShouldBe(expected.Latitude);
            actual.Accuracy.ShouldBe(expected.Accuracy);
            actual.Bearing.ShouldBe(expected.Bearing);
        }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(234641)]
    [InlineData(-1)]
    public async void GetManyByTeamId_TeamDoesNotExist_ReturnsNotFound(int id)
    {
        var factory = new CustomWebApplicationFactory<Program>();
        _client = factory.CreateClient();
        
        var response = await _client.GetAsync($"{_endpoint}{id}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
    
    [Theory]
    [InlineData(1, 2)]
    [InlineData(1, 3)]
    [InlineData(2, 1)]
    [InlineData(3, 4)]
    [InlineData(4, 6)]
    [InlineData(9, 3)]
    [InlineData(10, 3)]
    [InlineData(11, 3)]
    [InlineData(12, 4)]
    [InlineData(12, 3)]
    [InlineData(12, 2)]
    [InlineData(12, 1)]
    public async void GetManyByTeamId_ProvidedIdOfDifferentTeam_ReturnsForbidden(int senderPlayerId, int teamId)
    {
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        var response = await _client.GetAsync($"{_endpoint}{teamId}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}