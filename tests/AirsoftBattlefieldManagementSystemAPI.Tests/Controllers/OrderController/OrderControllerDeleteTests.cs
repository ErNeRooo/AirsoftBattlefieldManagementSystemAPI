using System.Net;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Controllers.OrderController;

public class OrderControllerDeleteTests
{
    private HttpClient _client;
    private string _endpoint = "order/id/";
        
    [Theory]
    [InlineData(3, 1)]
    [InlineData(9, 2)]
    [InlineData(2, 3)]
    public async void Delete_ValidId_ReturnsNoContent(int senderPlayerId, int orderId)
    {
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        var response = await _client.DeleteAsync($"{_endpoint}{orderId}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(112354)]
    public async void Delete_NotExistingOrder_ReturnsNotFound(int orderId)
    {
        var factory = new CustomWebApplicationFactory<Program>();
        _client = factory.CreateClient();
        
        var response = await _client.DeleteAsync($"{_endpoint}{orderId}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
    
    [Theory]
    [InlineData(10, 2)]
    [InlineData(11, 2)]
    [InlineData(12, 2)]
    [InlineData(3, 2)]
    [InlineData(4, 2)]
    [InlineData(9, 1)]
    [InlineData(4, 1)]
    [InlineData(9, 3)]
    [InlineData(3, 3)]
    public async void Delete_AsNotOfficer_ReturnsForbidden(int senderPlayerId, int orderId)
    {
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        var response = await _client.DeleteAsync($"{_endpoint}{orderId}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}