using System.Net;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.MapPing;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Tests.Helpers;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Controllers.MapPingController;

public class MapPingControllerCreateTests
{
    private HttpClient _client;
    private string _endpoint = "map-ping";
    
    public static IEnumerable<object[]> GetValidModelTestData()
    {
        var datetime = DateTimeOffset.Now;
        var tests = new List<PlayerMapPingTestData>
        {
            new()
            {
                SenderPlayerId = 1,
                BattleId = 1,
                Longitude = 21,
                Latitude = 45,
                Accuracy = 10,
                Bearing = 45,
                Time = datetime
            },
            new()
            {
                SenderPlayerId = 2,
                BattleId = 1,
                Longitude = 0,
                Latitude = 0,
                Accuracy = 0,
                Bearing = 0,
                Time = datetime
            },
            new()
            {
                SenderPlayerId = 4,
                BattleId = 2,
                Longitude = 4,
                Latitude = 7,
                Accuracy = 9,
                Bearing = 54,
                Time = datetime
            },
            new()
            {
                SenderPlayerId = 10,
                BattleId = 2,
                Longitude = 180,
                Latitude = 90,
                Accuracy = 9,
                Bearing = 360,
                Time = datetime
            },
            new()
            {
                SenderPlayerId = 11,
                BattleId = 2,
                Longitude = -180,
                Latitude = -90,
                Accuracy = 9,
                Bearing = 0,
                Time = datetime
            },
        };
        
        return tests.Select(x => new object[] { x });
    }
    
    public class PlayerMapPingTestData
    {
        public int SenderPlayerId { get; set; }
        public int BattleId { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public short Accuracy { get; set; }
        public short Bearing { get; set; }
        public DateTimeOffset Time { get; set; }
        public string Type { get; set; } = MapPingTypes.ENEMY;
    }
    
    [Theory]
    [MemberData(nameof(GetValidModelTestData))]
    public async void Create_ValidModel_ReturnsCreatedAndBattleDto(PlayerMapPingTestData testData)
    {
        var factory = new CustomWebApplicationFactory<Program>(testData.SenderPlayerId);
        _client = factory.CreateClient();

        var model = new PostMapPingDto
        {
            PlayerId = testData.SenderPlayerId,
            Latitude = testData.Latitude,
            Longitude = testData.Longitude,
            Accuracy = testData.Accuracy,
            Bearing = testData.Bearing,
            Time = testData.Time,
            Type = testData.Type
        };
        
        var response = await _client.PostAsync($"{_endpoint}", model.ToJsonHttpContent());
        var result = await response.Content.DeserializeFromHttpContentAsync<MapPingDto>();
        
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        result.MapPingId.ShouldNotBe(0);
        result.BattleId.ShouldBe(testData.BattleId);
        result.PlayerId.ShouldBe(testData.SenderPlayerId);
        result.Longitude.ShouldBe(testData.Longitude);
        result.Latitude.ShouldBe(testData.Latitude);
        result.Accuracy.ShouldBe(testData.Accuracy);
        result.Bearing.ShouldBe(testData.Bearing);
        result.Time.ShouldBe(testData.Time);
    }
    
    [Theory]
    [InlineData(12)]
    public async void Create_PlayerHasNoTeam_ReturnsForbidden(int senderPlayerId)
    {
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();

        var model = new PostMapPingDto
        {
            PlayerId = senderPlayerId,
            Latitude = 1,
            Longitude = 2,
            Accuracy = 3,
            Bearing = 4,
            Time = DateTimeOffset.Now,
            Type = MapPingTypes.ENEMY
        };
        
        var response = await _client.PostAsync($"{_endpoint}", model.ToJsonHttpContent());
        var result = await response.Content.DeserializeFromHttpContentAsync<MapPingDto>();
        
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
        result.ShouldBeNull();
    }
    
    [Theory]
    [InlineData(13)]
    public async void Create_NoBattle_ReturnsForbidden(int senderPlayerId)
    {
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();

        var model = new PostMapPingDto
        {
            PlayerId = senderPlayerId,
            Latitude = 1,
            Longitude = 2,
            Accuracy = 3,
            Bearing = 4,
            Time = DateTimeOffset.Now,
            Type = MapPingTypes.ENEMY
        };
        
        var response = await _client.PostAsync($"{_endpoint}", model.ToJsonHttpContent());
        var result = await response.Content.DeserializeFromHttpContentAsync<MapPingDto>();
        
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
        result.ShouldBeNull();
    }
}