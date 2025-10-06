using System.Net;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.MapPing;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Tests.Helpers;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Controllers.MapPingController;

public class MapPingControllerGetByIdTests
{
    private HttpClient _client;
    private string _endpoint = "map-ping/id/";
        
    public class PlayerMapPingTestData
    {
        public int SenderPlayerId { get; set; }
        public int MapPingId { get; set; }
        public int PlayerId { get; set; }
        public int BattleId { get; set; }
        public int LocationId { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public short Accuracy { get; set; }
        public short Bearing { get; set; }
        public string Type { get; set; }
    }
    
    public static IEnumerable<object[]> GetTests()
    {
        var tests = new List<PlayerMapPingTestData>
        {
            new()
            {
                SenderPlayerId = 1,
                MapPingId = 1,
                PlayerId = 1,
                BattleId = 1,
                LocationId = 1,
                Longitude = 21,
                Latitude = 45,
                Accuracy = 10,
                Bearing = 45,
                Type = MapPingTypes.ENEMY
            },
            new()
            {
                SenderPlayerId = 2,
                MapPingId = 2,
                PlayerId = 2,
                BattleId = 1,
                LocationId = 2,
                Longitude = 22.67m,
                Latitude = 46,
                Accuracy = 5,
                Bearing = 58,
                Type = MapPingTypes.ENEMY
            },
            new()
            {
                SenderPlayerId = 3,
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
                SenderPlayerId = 3,
                MapPingId = 4,
                PlayerId = 4,
                BattleId = 2,
                LocationId = 4,
                Longitude = 32.02m,
                Latitude = 40.4m,
                Accuracy = 7,
                Bearing = 268,
                Type = MapPingTypes.ENEMY
            },
            new()
            {
                SenderPlayerId = 4,
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
                SenderPlayerId = 4,
                MapPingId = 4,
                PlayerId = 4,
                BattleId = 2,
                LocationId = 4,
                Longitude = 32.02m,
                Latitude = 40.4m,
                Accuracy = 7,
                Bearing = 268,
                Type = MapPingTypes.ENEMY
            },
            new()
            {
                SenderPlayerId = 9,
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
                SenderPlayerId = 10,
                MapPingId = 11,
                PlayerId = 11,
                BattleId = 2,
                LocationId = 11,
                Longitude = -124.2213m,
                Latitude = -34.2137m,
                Accuracy = 1,
                Bearing = 180,
                Type = MapPingTypes.ENEMY
            },
            new()
            {
                SenderPlayerId = 11,
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
                SenderPlayerId = 11,
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
        };
        
        return tests.Select(x => new object[] { x });
    }

    
    [Theory]
    [MemberData(nameof(GetTests))]
    public async void GetById_Valid_ReturnsOkAndMapPingDto(PlayerMapPingTestData testData)
    {
        var factory = new CustomWebApplicationFactory<Program>(testData.SenderPlayerId);
        _client = factory.CreateClient();
        
        var response = await _client.GetAsync($"{_endpoint}{testData.MapPingId}");
        var result = await response.Content.DeserializeFromHttpContentAsync<MapPingDto>();
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        result.ShouldNotBeNull();
        result.MapPingId.ShouldBe(testData.MapPingId);
        result.PlayerId.ShouldBe(testData.PlayerId);
        result.BattleId.ShouldBe(testData.BattleId);
        result.LocationId.ShouldBe(testData.LocationId);
        result.Longitude.ShouldBe(testData.Longitude);
        result.Latitude.ShouldBe(testData.Latitude);
        result.Accuracy.ShouldBe(testData.Accuracy);
        result.Bearing.ShouldBe(testData.Bearing);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(234641)]
    [InlineData(-1)]
    public async void GetById_MapPingDoesNotExist_ReturnsNotFound(int id)
    {
        var factory = new CustomWebApplicationFactory<Program>();
        _client = factory.CreateClient();
        
        var response = await _client.GetAsync($"{_endpoint}{id}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
    
    [Theory]
    [InlineData(1, 2)]
    [InlineData(1, 3)]
    [InlineData(1, 9)]
    [InlineData(1, 11)]
    [InlineData(3, 9)]
    [InlineData(3, 11)]
    [InlineData(4, 9)]
    [InlineData(4, 10)]
    [InlineData(9, 4)]
    public async void GetById_MapPingIsInDifferentTeam_ReturnsForbidden(int senderPlayerId, int mapPingId)
    {
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        var response = await _client.GetAsync($"{_endpoint}{mapPingId}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}