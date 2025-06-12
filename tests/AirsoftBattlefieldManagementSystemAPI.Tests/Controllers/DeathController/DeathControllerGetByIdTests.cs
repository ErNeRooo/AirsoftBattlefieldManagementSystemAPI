using System.Net;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Death;
using AirsoftBattlefieldManagementSystemAPI.Tests.Helpers;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Controllers.DeathController;

public class DeathControllerGetByIdTests
{
    private HttpClient _client;
    private string _endpoint = "death/id/";
        
    public class PlayerDeathTestData
    {
        public int SenderPlayerId { get; set; }
        public int DeathId { get; set; }
        public int PlayerId { get; set; }
        public int RoomId { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public short Accuracy { get; set; }
        public short Bearing { get; set; }
        public DateTime Time { get; set; }
    }
    
    public static IEnumerable<object[]> GetTests()
    {
        var datetime = DateTime.Now;
        var tests = new List<PlayerDeathTestData>
        {
            new()
            {
                SenderPlayerId = 1,
                DeathId = 1,
                PlayerId = 1,
                RoomId = 1,
                Longitude = 21,
                Latitude = 45,
                Accuracy = 10,
                Bearing = 45,
                Time = datetime,
            },
            new()
            {
                SenderPlayerId = 1,
                DeathId = 2,
                PlayerId = 1,
                RoomId = 1,
                Longitude = 22.862341m,
                Latitude = 44.4325m,
                Accuracy = 15,
                Bearing = 80,
                Time = datetime,
            },
            new()
            {
                SenderPlayerId = 1,
                DeathId = 3,
                PlayerId = 2,
                RoomId = 1,
                Longitude = 56.23455m,
                Latitude = -86.4325m,
                Accuracy = 1500,
                Bearing = 203,
                Time = datetime,
            },
            new()
            {
                SenderPlayerId = 1,
                DeathId = 6,
                PlayerId = 1,
                RoomId = 1,
                Longitude = 18.22m,
                Latitude = 46.4m,
                Accuracy = 15,
                Bearing = 58,
                Time = datetime,
            },
            new()
            {
                SenderPlayerId = 2,
                DeathId = 3,
                PlayerId = 2,
                RoomId = 1,
                Longitude = 56.23455m,
                Latitude = -86.4325m,
                Accuracy = 1500,
                Bearing = 203,
                Time = datetime,
            },
            new()
            {
                SenderPlayerId = 2,
                DeathId = 2,
                PlayerId = 1,
                RoomId = 1,
                Longitude = 22.862341m,
                Latitude = 44.4325m,
                Accuracy = 15,
                Bearing = 80,
                Time = datetime,
            },
            new()
            {
                SenderPlayerId = 3,
                DeathId = 4,
                PlayerId = 3,
                RoomId = 2,
                Longitude = -32.02m,
                Latitude = -40.4m,
                Accuracy = 7,
                Bearing = 268,
                Time = datetime,
            },
            new()
            {
                SenderPlayerId = 6,
                DeathId = 4,
                PlayerId = 3,
                RoomId = 2,
                Longitude = -32.02m,
                Latitude = -40.4m,
                Accuracy = 7,
                Bearing = 268,
                Time = datetime,
            },
            new()
            {
                SenderPlayerId = 3,
                DeathId = 7,
                PlayerId = 4,
                RoomId = 2,
                Longitude = -124.2213m,
                Latitude = -34.2137m,
                Accuracy = 1,
                Bearing = 180,
                Time = datetime,
            },
            new()
            {
                SenderPlayerId = 4,
                DeathId = 4,
                PlayerId = 3,
                RoomId = 2,
                Longitude = -32.02m,
                Latitude = -40.4m,
                Accuracy = 7,
                Bearing = 268,
                Time = datetime,
            },
            new()
            {
                SenderPlayerId = 6,
                DeathId = 7,
                PlayerId = 4,
                RoomId = 2,
                Longitude = -124.2213m,
                Latitude = -34.2137m,
                Accuracy = 1,
                Bearing = 180,
                Time = datetime,
            },
        };
        
        return tests.Select(x => new object[] { x });
    }

    
    [Theory]
    [MemberData(nameof(GetTests))]
    public async void GetById_ValidId_ReturnsOkAndDeathDto(PlayerDeathTestData testData)
    {
        var factory = new CustomWebApplicationFactory<Program>(testData.SenderPlayerId);
        _client = factory.CreateClient();
        
        var response = await _client.GetAsync($"{_endpoint}{testData.DeathId}");
        var result = await response.Content.DeserializeFromHttpContentAsync<DeathDto>();
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        result.ShouldNotBeNull();
        result.DeathId.ShouldBe(testData.DeathId);
        result.PlayerId.ShouldBe(testData.PlayerId);
        result.RoomId.ShouldBe(testData.RoomId);
        result.Longitude.ShouldBe(testData.Longitude);
        result.Latitude.ShouldBe(testData.Latitude);
        result.Accuracy.ShouldBe(testData.Accuracy);
        result.Bearing.ShouldBe(testData.Bearing);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(234641)]
    [InlineData(-1)]
    public async void GetById_DeathDoesNotExist_ReturnsNotFound(int id)
    {
        var factory = new CustomWebApplicationFactory<Program>();
        _client = factory.CreateClient();
        
        var response = await _client.GetAsync($"{_endpoint}{id}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
    
    [Theory]
    [InlineData(1, 4)]
    [InlineData(1, 5)]
    [InlineData(2, 4)]
    [InlineData(3, 1)]
    [InlineData(4, 3)]
    [InlineData(5, 4)]
    [InlineData(5, 3)]
    [InlineData(5, 4)]
    [InlineData(5, 7)]
    public async void GetById_DeathIsInDifferentRoom_ReturnsForbidden(int senderPlayerId, int deathId)
    {
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        var response = await _client.GetAsync($"{_endpoint}{deathId}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}