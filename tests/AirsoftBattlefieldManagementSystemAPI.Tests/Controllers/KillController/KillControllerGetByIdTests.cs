using System.Net;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Kill;
using AirsoftBattlefieldManagementSystemAPI.Tests.Helpers;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Controllers.KillController;

public class KillControllerGetByIdTests
{
    private HttpClient _client;
    private string _endpoint = "kill/id/";
        
    public class PlayerKillTestData
    {
        public int SenderPlayerId { get; set; }
        public int KillId { get; set; }
        public int PlayerId { get; set; }
        public int BattleId { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public short Accuracy { get; set; }
        public short Bearing { get; set; }
        public DateTime Time { get; set; }
    }
    
    public static IEnumerable<object[]> GetTests()
    {
        var datetime = DateTime.Now;
        var tests = new List<PlayerKillTestData>
        {
            new()
            {
                SenderPlayerId = 1,
                KillId = 1,
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
                KillId = 2,
                PlayerId = 1,
                BattleId = 1,
                Longitude = 22.862341m,
                Latitude = 44.4325m,
                Accuracy = 15,
                Bearing = 80,
                Time = datetime,
            },
            new()
            {
                SenderPlayerId = 1,
                KillId = 3,
                PlayerId = 2,
                BattleId = 1,
                Longitude = 56.23455m,
                Latitude = -86.4325m,
                Accuracy = 1500,
                Bearing = 203,
                Time = datetime,
            },
            new()
            {
                SenderPlayerId = 1,
                KillId = 6,
                PlayerId = 1,
                BattleId = 1,
                Longitude = 18.22m,
                Latitude = 46.4m,
                Accuracy = 15,
                Bearing = 58,
                Time = datetime,
            },
            new()
            {
                SenderPlayerId = 2,
                KillId = 3,
                PlayerId = 2,
                BattleId = 1,
                Longitude = 56.23455m,
                Latitude = -86.4325m,
                Accuracy = 1500,
                Bearing = 203,
                Time = datetime,
            },
            new()
            {
                SenderPlayerId = 2,
                KillId = 2,
                PlayerId = 1,
                BattleId = 1,
                Longitude = 22.862341m,
                Latitude = 44.4325m,
                Accuracy = 15,
                Bearing = 80,
                Time = datetime,
            },
            new()
            {
                SenderPlayerId = 3,
                KillId = 4,
                PlayerId = 3,
                BattleId = 2,
                Longitude = -32.02m,
                Latitude = -40.4m,
                Accuracy = 7,
                Bearing = 268,
                Time = datetime,
            },
            new()
            {
                SenderPlayerId = 6,
                KillId = 4,
                PlayerId = 3,
                BattleId = 2,
                Longitude = -32.02m,
                Latitude = -40.4m,
                Accuracy = 7,
                Bearing = 268,
                Time = datetime,
            },
            new()
            {
                SenderPlayerId = 3,
                KillId = 7,
                PlayerId = 4,
                BattleId = 2,
                Longitude = -124.2213m,
                Latitude = -34.2137m,
                Accuracy = 1,
                Bearing = 180,
                Time = datetime,
            },
            new()
            {
                SenderPlayerId = 4,
                KillId = 4,
                PlayerId = 3,
                BattleId = 2,
                Longitude = -32.02m,
                Latitude = -40.4m,
                Accuracy = 7,
                Bearing = 268,
                Time = datetime,
            },
            new()
            {
                SenderPlayerId = 6,
                KillId = 7,
                PlayerId = 4,
                BattleId = 2,
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
    public async void GetById_ValidId_ReturnsOkAndKillDto(PlayerKillTestData testData)
    {
        var factory = new CustomWebApplicationFactory<Program>(testData.SenderPlayerId);
        _client = factory.CreateClient();
        
        var response = await _client.GetAsync($"{_endpoint}{testData.KillId}");
        var result = await response.Content.DeserializeFromHttpContentAsync<KillDto>();
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        result.ShouldNotBeNull();
        result.KillId.ShouldBe(testData.KillId);
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
    public async void GetById_KillDoesNotExist_ReturnsNotFound(int id)
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
    public async void GetById_KillIsInDifferentRoom_ReturnsForbidden(int senderPlayerId, int killId)
    {
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        var response = await _client.GetAsync($"{_endpoint}{killId}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}