using System.Net;
using System.Security.Claims;
using AirsoftBattlefieldManagementSystemAPI.Tests.Helpers;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Account;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Controllers.AccountController;

public class AccountControllerDeleteTests
{
    private HttpClient _client;
    private readonly string _endpoint = "/account/id/";

    public AccountControllerDeleteTests()
    {
        var factory = new CustomWebApplicationFactory<Program>();
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Delete_ValidId_ReturnsNoContent()
    {
        // arrange
        var factory = new CustomWebApplicationFactory<Program>(2);
        _client = factory.CreateClient();
        
        // act
        var response = await _client.DeleteAsync($"{_endpoint}{2137}");
        
        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    } 
    
    [Fact]
    public async Task Delete_AccountDoesNotExist_ReturnsNotFound()
    {
        // act
        var response = await _client.DeleteAsync($"{_endpoint}{12491938}");
        
        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    } 
    
    [Theory]
    [InlineData(1, 1)]
    [InlineData(1, 2)]
    [InlineData(2, 1)]
    [InlineData(2, 2)]
    public async Task Delete_AccountOfOtherPlayer_ReturnsForbidden(int accountId, int playerId)
    {
        // arrange
        var factory = new CustomWebApplicationFactory<Program>(playerId);
        _client = factory.CreateClient();
        
        // act
        var response = await _client.DeleteAsync($"{_endpoint}{accountId}");
        
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