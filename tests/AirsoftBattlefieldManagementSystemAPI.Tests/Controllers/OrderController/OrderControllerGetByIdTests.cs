using System.Net;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Order;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Tests.Helpers;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Controllers.OrderController;

public class OrderControllerGetByIdTests
{
    private HttpClient _client;
    private string _endpoint = "order/id/";
        
    public class PlayerOrderTestData
    {
        public int SenderPlayerId { get; set; }
        public int OrderId { get; set; }
        public int PlayerId { get; set; }
        public int BattleId { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public short Accuracy { get; set; }
        public short Bearing { get; set; }
        public string Type { get; set; }
    }
    
    public static IEnumerable<object[]> GetTests()
    {
        var tests = new List<PlayerOrderTestData>
        {
            new()
            {
                SenderPlayerId = 2,
                OrderId = 3,
                PlayerId = 2,
                BattleId = 1,
                Longitude = 18.22m,
                Latitude = 46.4m,
                Accuracy = 15,
                Bearing = 58,
                Type = OrderTypes.DEFEND
            },
            new()
            {
                SenderPlayerId = 3,
                OrderId = 1,
                PlayerId = 4,
                BattleId = 2,
                Longitude = -2.2m,
                Latitude = -0.2137m,
                Accuracy = 1,
                Bearing = 180,
                Type = OrderTypes.MOVE
            },
            new()
            {
                SenderPlayerId = 9,
                OrderId = 2,
                PlayerId = 10,
                BattleId = 2,
                Longitude = -124.2213m,
                Latitude = -34.2137m,
                Accuracy = 1,
                Bearing = 180,
                Type = OrderTypes.DEFEND
            },
        };
        
        return tests.Select(x => new object[] { x });
    }

    
    [Theory]
    [MemberData(nameof(GetTests))]
    public async void GetById_Valid_ReturnsOkAndOrderDto(PlayerOrderTestData testData)
    {
        var factory = new CustomWebApplicationFactory<Program>(testData.SenderPlayerId);
        _client = factory.CreateClient();
        
        var response = await _client.GetAsync($"{_endpoint}{testData.OrderId}");
        var result = await response.Content.DeserializeFromHttpContentAsync<OrderDto>();
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        result.ShouldNotBeNull();
        result.OrderId.ShouldBe(testData.OrderId);
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
    public async void GetById_OrderDoesNotExist_ReturnsNotFound(int id)
    {
        var factory = new CustomWebApplicationFactory<Program>();
        _client = factory.CreateClient();
        
        var response = await _client.GetAsync($"{_endpoint}{id}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
    
    [Theory]
    [InlineData(1, 1)]
    [InlineData(1, 3)]
    [InlineData(9, 3)]
    public async void GetById_OrderIsInDifferentTeam_ReturnsForbidden(int senderPlayerId, int orderId)
    {
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        var response = await _client.GetAsync($"{_endpoint}{orderId}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}