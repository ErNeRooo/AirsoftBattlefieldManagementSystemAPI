using System.Net;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Battle;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Location;
using AirsoftBattlefieldManagementSystemAPI.Tests.Helpers;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Controllers.LocationController;

public class LocationControllerCreateTests
{
    private HttpClient _client;
    private string _endpoint = "location/";
    
    public static IEnumerable<object[]> GetTests()
    {
        var datetime = DateTime.Now;
        var tests = new List<PlayerLocationTestData>
        {
            new()
            {
                SenderPlayerId = 1,
                BattleId = 1,
                Longitude = 21,
                Latitude = 45,
                Accuracy = 10,
                Bearing = 45,
                Time = datetime,
            },
            new()
            {
                SenderPlayerId = 2,
                BattleId = 1,
                Longitude = 53,
                Latitude = 34,
                Accuracy = 12,
                Bearing = 89,
                Time = datetime,
            },
            new()
            {
                SenderPlayerId = 3,
                BattleId = 2,
                Longitude = 53,
                Latitude = 34,
                Accuracy = 12,
                Bearing = 89,
                Time = datetime,
            },
            new()
            {
                SenderPlayerId = 4,
                BattleId = 2,
                Longitude = 53,
                Latitude = 34,
                Accuracy = 12,
                Bearing = 89,
                Time = datetime,
            },
            new()
            {
                SenderPlayerId = 6,
                BattleId = 2,
                Longitude = 53,
                Latitude = 34,
                Accuracy = 12,
                Bearing = 89,
                Time = datetime,
            },
        };
        
        return tests.Select(x => new object[] { x });
    }
    
    public class PlayerLocationTestData
    {
        public int SenderPlayerId { get; set; }
        public int? BattleId { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public short Accuracy { get; set; }
        public short Bearing { get; set; }
        public DateTime Time { get; set; }
    }
    
    [Theory]
    [MemberData(nameof(GetTests))]
    public async void Create_ValidModel_ReturnsCreatedAndBattleDto(PlayerLocationTestData testData)
    {
        var factory = new CustomWebApplicationFactory<Program>(testData.SenderPlayerId);
        _client = factory.CreateClient();

        var model = new PostLocationDto()
        {
            Latitude = testData.Latitude,
            Longitude = testData.Longitude,
            Accuracy = testData.Accuracy,
            Bearing = testData.Bearing,
            Time = testData.Time,
        };
        
        var response = await _client.PostAsync($"{_endpoint}", model.ToJsonHttpContent());
        var result = await response.Content.DeserializeFromHttpContentAsync<LocationDto>();
        
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        result.LocationId.ShouldNotBe(0);
        result.BattleId.ShouldBe(testData.BattleId);
        result.PlayerId.ShouldBe(testData.SenderPlayerId);
        result.Longitude.ShouldBe(testData.Longitude);
        result.Latitude.ShouldBe(testData.Latitude);
        result.Accuracy.ShouldBe(testData.Accuracy);
        result.Bearing.ShouldBe(testData.Bearing);
        result.Time.ShouldBe(testData.Time);
    }
}