using System.Net;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Controllers.ZoneController;

public class ZoneControllerDeleteTests
{
    private HttpClient _client;
    private string _endpoint = "zone/";

    [Theory]
    [InlineData(1, 1)]
    [InlineData(3, 2)]
    [InlineData(3, 3)]
    public async void Delete_Valid_ReturnsNoContent(int senderPlayerId, int zoneId)
    {
        // Arrange
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();

        // Act
        var response = await _client.DeleteAsync($"{_endpoint}id/{zoneId}");
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(234641)]
    [InlineData(-1)]
    public async void Delete_ForNotExistingZone_ReturnsNotFound(int id)
    {
        // Arrange
        var factory = new CustomWebApplicationFactory<Program>();
        _client = factory.CreateClient();

        // Act
        var response = await _client.DeleteAsync($"{_endpoint}id/{id}");
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
    
    [Theory]
    [InlineData(2, 1)]
    [InlineData(4, 2)]
    [InlineData(9, 2)]
    [InlineData( 6, 3)]
    [InlineData(10, 3)]
    [InlineData(12, 3)]
    public async void Delete_PlayerIsNotRoomAdmin_ReturnsForbidden(int senderPlayerId, int zoneId)
    {
        // Arrange
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();

        // Act
        var response = await _client.DeleteAsync($"{_endpoint}id/{zoneId}");
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
    
    [Theory]
    [InlineData(1, 2)]
    [InlineData(1, 3)]
    [InlineData(3, 1)]
    [InlineData(2, 2)]
    [InlineData(9, 1)]
    [InlineData(10, 1)]
    [InlineData(12, 1)]
    public async void Delete_PlayerIsNotInTheSameRoom_ReturnsForbidden(int senderPlayerId, int zoneId)
    {
        // Arrange
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();

        // Act
        var response = await _client.DeleteAsync($"{_endpoint}id/{zoneId}");
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}