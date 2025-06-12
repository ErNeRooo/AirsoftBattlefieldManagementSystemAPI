using System.Net;
using System.Security.Claims;
using AirsoftBattlefieldManagementSystemAPI.Tests.Helpers;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Account;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Controllers.AccountController;

public class AccountControllerDeleteTests
{
    private HttpClient _client;
    private readonly string _endpoint = "account";

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
        var response = await _client.DeleteAsync($"{_endpoint}");
        
        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    } 
    
    [Theory]
    [InlineData(5)]
    public async Task Delete_PlayerWithoutAccount_ReturnsNotFound(int senderPlayerId)
    {
        // arrange
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        // act
        var response = await _client.DeleteAsync($"{_endpoint}");
        
        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    } 
}