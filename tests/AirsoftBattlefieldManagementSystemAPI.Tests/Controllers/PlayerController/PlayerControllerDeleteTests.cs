using System.Net;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Controllers.PlayerController;

public class PlayerControllerDeleteTests
{
    private HttpClient _client;
    private string _endpoint = "/player/id/";

    public PlayerControllerDeleteTests()
    {
        CustomWebApplicationFactory<Program> factory = new CustomWebApplicationFactory<Program>();
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Delete_ValidId_ReturnsNoContent()
    {
        // act
        var response = await _client.DeleteAsync($"{_endpoint}{1}");
        
        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    } 
    
    [Fact]
    public async Task Delete_PlayerDoesNotExist_ReturnsNotFound()
    {
        // act
        var response = await _client.DeleteAsync($"{_endpoint}{12491938}");
        
        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    } 
    
    [Theory]
    [InlineData(2, 1)]
    [InlineData(1, 2)]
    [InlineData(2, 4)]
    [InlineData(3, 1)]
    public async Task Delete_OtherPlayer_ReturnsForbidden(int targetPlayerId, int senderPlayerId)
    {
        // arrange
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        // act
        var response = await _client.DeleteAsync($"{_endpoint}{targetPlayerId}");
        
        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    } 
    
    [Fact]
    public async Task Delete_NotValidId_ReturnsBadRequest()
    {
        // act
        var response = await _client.DeleteAsync($"{_endpoint}{"2 esdw"}");
        
        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    } 
}