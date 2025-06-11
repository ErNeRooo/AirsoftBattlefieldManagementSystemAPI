using System.Net;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Location;
using AirsoftBattlefieldManagementSystemAPI.Tests.Helpers;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Controllers.LocationController;

public class LocationControllerUpdateTests
{
    private HttpClient _client;
    private string _endpoint = "location/id/";
    
    public LocationControllerUpdateTests()
    {
        CustomWebApplicationFactory<Program> factory = new CustomWebApplicationFactory<Program>();
        _client = factory.CreateClient();
    }
    
    public class PlayerLocationTestData
    {
        public int SenderPlayerId { get; set; }
        public int LocationId { get; set; }
        public int RoomId { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
        public short? Accuracy { get; set; }
        public short? Bearing { get; set; }
        public DateTime? Time { get; set; }
    }
    
    public static IEnumerable<object[]> GetDataFor_ValidModel()
    {
        var datetime = DateTime.Now;
        var tests = new List<PlayerLocationTestData>
        {
            new()
            {
                SenderPlayerId = 1,
                LocationId = 1,
                RoomId = 1,
                Longitude = 86,
                Latitude = 43,
                Accuracy = 2,
                Bearing = 5,
                Time = datetime,
            },
            new()
            {
                SenderPlayerId = 1,
                LocationId = 2,
                RoomId = 1,
                Longitude = 75,
                Latitude = 46,
                Accuracy = 123,
                Bearing = 282,
                Time = datetime,
            },
            new()
            {
                SenderPlayerId = 2,
                LocationId = 4,
                RoomId = 1,
                Longitude = 86,
                Latitude = 87,
                Accuracy = 88,
                Bearing = 89,
                Time = datetime,
            },
            new()
            {
                SenderPlayerId = 3,
                LocationId = 5,
                RoomId = 2,
                Longitude = 1,
                Latitude = 1,
                Accuracy = 1,
                Bearing = 1,
                Time = datetime,
            },
            new()
            {
                SenderPlayerId = 6,
                LocationId = 7,
                RoomId = 2,
                Longitude = 6,
                Latitude = 6,
                Accuracy = 6,
                Bearing = 6,
                Time = datetime,
            },
        };
        
        return tests.Select(x => new object[] { x });
    }
    
    public static IEnumerable<object[]> GetDataFor_NotAllFieldsSpecified()
    {
        var datetime = DateTime.Now;
        var tests = new List<PlayerLocationTestData>
        {
            new()
            {
                SenderPlayerId = 1,
                LocationId = 1,
                RoomId = 1,
                Longitude = null,
                Latitude = null,
                Accuracy = null,
                Bearing = null,
                Time = null,
            },
            new()
            {
                SenderPlayerId = 3,
                LocationId = 5,
                RoomId = 2,
                Longitude = null,
                Latitude = null,
                Accuracy = null,
                Bearing = null,
                Time = null,
            },
            new()
            {
                SenderPlayerId = 1,
                LocationId = 2,
                RoomId = 1,
                Longitude = null,
                Latitude = 24,
                Accuracy = null,
                Bearing = null,
                Time = null,
            },
            new()
            {
                SenderPlayerId = 6,
                LocationId = 7,
                RoomId = 2,
                Longitude = null,
                Latitude = null,
                Accuracy = null,
                Bearing = 10,
                Time = null,
            },
            new()
            {
                SenderPlayerId = 1,
                LocationId = 1,
                RoomId = 1,
                Longitude = 155,
                Latitude = null,
                Accuracy = null,
                Bearing = null,
                Time = null,
            },
            new()
            {
                SenderPlayerId = 1,
                LocationId = 1,
                RoomId = 1,
                Longitude = null,
                Latitude = null,
                Accuracy = 6546,
                Bearing = null,
                Time = null,
            },
            new()
            {
                SenderPlayerId = 1,
                LocationId = 1,
                RoomId = 1,
                Longitude = null,
                Latitude = null,
                Accuracy = 6546,
                Bearing = 359,
                Time = null,
            },
            new()
            {
                SenderPlayerId = 1,
                LocationId = 1,
                RoomId = 1,
                Longitude = null,
                Latitude = null,
                Accuracy = 6546,
                Bearing = 359,
                Time = datetime,
            },
        };
        
        return tests.Select(x => new object[] { x });
    }

    
    // Update_ValidId_ReturnsOkAndLocationDto
    [Theory]
    [MemberData(nameof(GetDataFor_ValidModel))]
    public async void Update_ValidModel_ReturnsCreatedAndBattleDto(PlayerLocationTestData testData)
    {
        var factory = new CustomWebApplicationFactory<Program>(testData.SenderPlayerId);
        _client = factory.CreateClient();

        var model = new PutLocationDto()
        {
            Latitude = testData.Latitude,
            Longitude = testData.Longitude,
            Accuracy = testData.Accuracy,
            Bearing = testData.Bearing,
            Time = testData.Time,
        };
        
        var response = await _client.PutAsync($"{_endpoint}{testData.LocationId}", model.ToJsonHttpContent());
        var result = await response.Content.DeserializeFromHttpContentAsync<LocationDto>();
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        result.LocationId.ShouldNotBe(0);
        result.RoomId.ShouldBe(testData.RoomId);
        result.PlayerId.ShouldBe(testData.SenderPlayerId);
        result.Longitude.ShouldBe((int)testData.Longitude);
        result.Latitude.ShouldBe((int)testData.Latitude);
        result.Accuracy.ShouldBe((short)testData.Accuracy);
        result.Bearing.ShouldBe((short)testData.Bearing);
        result.Time.ShouldBe((DateTime)testData.Time);
    }
    
    // Update_NotAllFieldsSpecified_ReturnsOkAndLocationDto
    [Theory]
    [MemberData(nameof(GetDataFor_NotAllFieldsSpecified))]
    public async void Update_NotAllFieldsSpecified_ReturnsOkAndLocationDto(PlayerLocationTestData testData)
    {
        var factory = new CustomWebApplicationFactory<Program>(testData.SenderPlayerId);
        _client = factory.CreateClient();

        var model = new PutLocationDto()
        {
            Latitude = testData.Latitude,
            Longitude = testData.Longitude,
            Accuracy = testData.Accuracy,
            Bearing = testData.Bearing,
            Time = testData.Time,
        };
        
        var responseFromGet = await _client.GetAsync($"{_endpoint}{testData.LocationId}");
        var resultFromGet = await responseFromGet.Content.DeserializeFromHttpContentAsync<LocationDto>();
        
        responseFromGet.StatusCode.ShouldBe(HttpStatusCode.OK);
        
        var response = await _client.PutAsync($"{_endpoint}{testData.LocationId}", model.ToJsonHttpContent());
        var result = await response.Content.DeserializeFromHttpContentAsync<LocationDto>();
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        result.LocationId.ShouldNotBe(0);
        result.RoomId.ShouldBe(testData.RoomId);
        result.PlayerId.ShouldBe(testData.SenderPlayerId);
        result.Longitude.ShouldBe(testData.Longitude ?? resultFromGet.Longitude);
        result.Latitude.ShouldBe(testData.Latitude ?? resultFromGet.Latitude);
        result.Accuracy.ShouldBe(testData.Accuracy ?? resultFromGet.Accuracy);
        result.Bearing.ShouldBe(testData.Bearing ?? resultFromGet.Bearing);
        result.Time.ShouldBe(testData.Time ?? resultFromGet.Time);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(234641)]
    [InlineData(-1)]
    public async void Update_NotExistingLocation_ReturnsNotFound(int id)
    {
        var model = new PutLocationDto();
        
        var response = await _client.PutAsync($"{_endpoint}{id}", model.ToJsonHttpContent());
        
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
    
    [Theory]
    [InlineData(1, 4)]
    [InlineData(2, 1)]
    [InlineData(3, 7)]
    [InlineData(4, 5)]
    [InlineData(4, 1)]
    [InlineData(1, 7)]
    [InlineData(6, 4)]
    [InlineData(5, 4)]
    [InlineData(5, 7)]
    public async void Update_LocationOfOtherPlayer_ReturnsForbidden(int senderPlayerId, int locationId)
    {
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        var model = new PutLocationDto();
        
        var response = await _client.PutAsync($"{_endpoint}{locationId}", model.ToJsonHttpContent());
        
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}