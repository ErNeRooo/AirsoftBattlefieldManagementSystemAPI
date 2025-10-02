using System.Net;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Order;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Tests.Helpers;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Controllers.OrderController;

public class OrderControllerCreateTests
{
    private HttpClient _client;
    private string _endpoint = "order";
    
    public static IEnumerable<object[]> GetValidModelTestData()
    {
        var datetime = DateTimeOffset.Now;
        var tests = new List<PlayerOrderTestData>
        {
            new()
            {
                SenderPlayerId = 1,
                TargetPlayerId = 1,
                BattleId = 1,
                Longitude = 21,
                Latitude = 45,
                Accuracy = 10,
                Bearing = 45,
                Time = datetime,
                Type = OrderTypes.MOVE
            },
            new()
            {
                SenderPlayerId = 2,
                TargetPlayerId = 2,
                BattleId = 1,
                Longitude = 53,
                Latitude = 34,
                Accuracy = 12,
                Bearing = 89,
                Time = datetime,
                Type = OrderTypes.DEFEND
            },
            new()
            {
                SenderPlayerId = 3,
                TargetPlayerId = 4,
                BattleId = 2,
                Longitude = 53,
                Latitude = 34,
                Accuracy = 12,
                Bearing = 89,
                Time = datetime,
                Type = OrderTypes.MOVE
            },
            new()
            {
                SenderPlayerId = 9,
                TargetPlayerId = 10,
                BattleId = 2,
                Longitude = 53,
                Latitude = 34,
                Accuracy = 12,
                Bearing = 89,
                Time = datetime,
                Type = OrderTypes.DEFEND
            },
            new()
            {
                SenderPlayerId = 9,
                TargetPlayerId = 11,
                BattleId = 2,
                Longitude = 53,
                Latitude = 34,
                Accuracy = 12,
                Bearing = 89,
                Time = datetime,
                Type = OrderTypes.MOVE
            },
        };
        
        return tests.Select(x => new object[] { x });
    }
    
    public class PlayerOrderTestData
    {
        public int SenderPlayerId { get; set; }
        public int TargetPlayerId { get; set; }
        public int BattleId { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public short Accuracy { get; set; }
        public short Bearing { get; set; }
        public DateTimeOffset Time { get; set; }
        public string Type { get; set; }
    }
    
    [Theory]
    [MemberData(nameof(GetValidModelTestData))]
    public async void Create_ValidModel_ReturnsCreatedAndBattleDto(PlayerOrderTestData testData)
    {
        var factory = new CustomWebApplicationFactory<Program>(testData.SenderPlayerId);
        _client = factory.CreateClient();

        var model = new PostOrderDto
        {
            PlayerId = testData.TargetPlayerId,
            Latitude = testData.Latitude,
            Longitude = testData.Longitude,
            Accuracy = testData.Accuracy,
            Bearing = testData.Bearing,
            Time = testData.Time,
            Type = testData.Type
        };
        
        var response = await _client.PostAsync($"{_endpoint}", model.ToJsonHttpContent());
        var result = await response.Content.DeserializeFromHttpContentAsync<OrderDto>();
        
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        result.OrderId.ShouldNotBe(0);
        result.BattleId.ShouldBe(testData.BattleId);
        result.PlayerId.ShouldBe(testData.TargetPlayerId);
        result.Longitude.ShouldBe(testData.Longitude);
        result.Latitude.ShouldBe(testData.Latitude);
        result.Accuracy.ShouldBe(testData.Accuracy);
        result.Bearing.ShouldBe(testData.Bearing);
        result.Time.ShouldBe(testData.Time);
    }
    
    [Theory]
    [InlineData(10, 9)]
    [InlineData(10, 11)]
    [InlineData(10, 4)]
    [InlineData(10, 12)]
    [InlineData(4, 3)]
    public async void Create_NotTeamOfficer_ReturnsForbidden(int senderPlayerId, int targetPlayerId)
    {
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();

        var model = new PostOrderDto
        {
            PlayerId = targetPlayerId,
            Latitude = 1,
            Longitude = 2,
            Accuracy = 3,
            Bearing = 4,
            Time = DateTimeOffset.Now,
            Type = OrderTypes.MOVE
        };
        
        var response = await _client.PostAsync($"{_endpoint}", model.ToJsonHttpContent());
        var result = await response.Content.DeserializeFromHttpContentAsync<OrderDto>();
        
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
        result.ShouldBeNull();
    }
    
    [Theory]
    [InlineData(1, 2)]
    [InlineData(3, 6)]
    [InlineData(3, 7)]
    [InlineData(9, 6)]
    [InlineData(3, 12)]
    public async void Create_OrderToPlayerFromDifferentTeam_ReturnsForbidden(int senderPlayerId, int targetPlayerId)
    {
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();

        var model = new PostOrderDto
        {
            PlayerId = targetPlayerId,
            Latitude = 1,
            Longitude = 2,
            Accuracy = 3,
            Bearing = 4,
            Time = DateTimeOffset.Now,
            Type = OrderTypes.MOVE
        };
        
        var response = await _client.PostAsync($"{_endpoint}", model.ToJsonHttpContent());
        var result = await response.Content.DeserializeFromHttpContentAsync<OrderDto>();
        
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
        result.ShouldBeNull();
    }
    
    [Theory]
    [InlineData(13, 13)]
    public async void Create_NoBattle_ReturnsForbidden(int senderPlayerId, int targetPlayerId)
    {
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();

        var model = new PostOrderDto
        {
            PlayerId = targetPlayerId,
            Latitude = 1,
            Longitude = 2,
            Accuracy = 3,
            Bearing = 4,
            Time = DateTimeOffset.Now,
            Type = OrderTypes.MOVE
        };
        
        var response = await _client.PostAsync($"{_endpoint}", model.ToJsonHttpContent());
        var result = await response.Content.DeserializeFromHttpContentAsync<OrderDto>();
        
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
        result.ShouldBeNull();
    }
}