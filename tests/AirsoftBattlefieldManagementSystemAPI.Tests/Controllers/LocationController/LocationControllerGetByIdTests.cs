using System.Net;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Location;
using AirsoftBattlefieldManagementSystemAPI.Tests.Helpers;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Controllers.LocationController;

public class LocationControllerGetByIdTests
{
    private HttpClient _client;
    private string _endpoint = "location/id/";
    
    public static IEnumerable<object[]> GetTests()
    {
        var datetime = DateTime.Now;
        var tests = new List<PlayerLocationTestData>
        {
            new()
            {
                SenderPlayerId = 1,
                PlayerLocationId = 1,
                LocationId = 1,
                PlayerId = 1,
                BattleId = 1,
                Longitude = 21,
                Latitude = 45,
                Accuracy = 10,
                Bearing = 45,
                Time = datetime,
            },
            new()
            {
                SenderPlayerId = 1,
                PlayerLocationId = 2,
                LocationId = 2,
                PlayerId = 1,
                BattleId = 1,
                Longitude = 22.67m,
                Latitude = 46,
                Accuracy = 5,
                Bearing = 58,
                Time = datetime.AddSeconds(5),
            },
            new()
            {
                SenderPlayerId = 2,
                PlayerLocationId = 4,
                LocationId = 4,
                PlayerId = 2,
                BattleId = 1,
                Longitude = 32.02m,
                Latitude = 40.4m,
                Accuracy = 7,
                Bearing = 268,
                Time = datetime.AddSeconds(11),
            },
            new()
            {
                SenderPlayerId = 3,
                PlayerLocationId = 5,
                LocationId = 5,
                PlayerId = 3,
                BattleId = 2,
                Longitude = -32.02m,
                Latitude = -40.4m,
                Accuracy = 7,
                Bearing = 268,
                Time = datetime.AddSeconds(3),
            },
            new()
            {
                SenderPlayerId = 4,
                PlayerLocationId = 5,
                LocationId = 5,
                PlayerId = 3,
                BattleId = 2,
                Longitude = -32.02m,
                Latitude = -40.4m,
                Accuracy = 7,
                Bearing = 268,
                Time = datetime.AddSeconds(3),
            },
            new()
            {
                SenderPlayerId = 6,
                PlayerLocationId = 7,
                LocationId = 7,
                PlayerId = 6,
                BattleId = 2,
                Longitude = -2.02m,
                Latitude = -4.42m,
                Accuracy = 1000,
                Bearing = 230,
                Time = DateTime.Now.AddSeconds(4)
            }
        };
        
        return tests.Select(x => new object[] { x });
    }
    
    public class PlayerLocationTestData
    {
        public int SenderPlayerId { get; set; }
        public int PlayerLocationId { get; set; }
        public int LocationId { get; set; }
        public int PlayerId { get; set; }
        public int? BattleId { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public short Accuracy { get; set; }
        public short Bearing { get; set; }
        public DateTime Time { get; set; }
    }
    
    [Theory]
    [MemberData(nameof(GetTests))]
    public async void GetById_ValidId_ReturnsOkAndLocationDto(PlayerLocationTestData testData)
    {
        var factory = new CustomWebApplicationFactory<Program>(testData.SenderPlayerId);
        _client = factory.CreateClient();
        
        var response = await _client.GetAsync($"{_endpoint}{testData.LocationId}");
        var result = await response.Content.DeserializeFromHttpContentAsync<LocationDto>();
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        result.ShouldNotBeNull();
        result.LocationId.ShouldBe(testData.LocationId);
        result.PlayerId.ShouldBe(testData.PlayerId);
        result.BattleId.ShouldBe(testData.BattleId);
        result.Longitude.ShouldBe(testData.Longitude);
        result.Latitude.ShouldBe(testData.Latitude);
        result.Accuracy.ShouldBe(testData.Accuracy);
        result.Bearing.ShouldBe(testData.Bearing);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(234641)]
    [InlineData(-1)]
    public async void GetById_LocationDoesNotExist_ReturnsNotFound(int id)
    {
        CustomWebApplicationFactory<Program> factory = new CustomWebApplicationFactory<Program>();
        _client = factory.CreateClient();
        
        var response = await _client.GetAsync($"{_endpoint}{id}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
    
    [Theory]
    [InlineData(1, 4)]
    [InlineData(1, 5)]
    [InlineData(1, 6)]
    [InlineData(1, 7)]
    [InlineData(2, 1)]
    [InlineData(2, 2)]
    [InlineData(2, 6)]
    [InlineData(2, 7)]
    public async void GetById_LocationIsInDifferentTeam_ReturnsForbidden(int senderPlayerId, int locationId)
    {
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        var response = await _client.GetAsync($"{_endpoint}{locationId}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}