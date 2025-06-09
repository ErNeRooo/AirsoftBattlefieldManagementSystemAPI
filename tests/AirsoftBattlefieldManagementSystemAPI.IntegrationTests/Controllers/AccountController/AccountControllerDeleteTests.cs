using System.Net;
using AirsoftBattlefieldManagementSystemAPI.IntegrationTests.Helpers;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Account;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.IntegrationTests.Controllers.AccountController;

public class AccountControllerDeleteTests
{
    private HttpClient _client;

    public AccountControllerDeleteTests()
    {
        CustomWebApplicationFactory<Program> factory = new CustomWebApplicationFactory<Program>();
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Delete_ValidId_ReturnsNoContent()
    {
        // act
        var response = await _client.DeleteAsync($"/account/id/{2137}");
        
        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    } 
    
    [Fact]
    public async Task Delete_AccountDoesNotExist_ReturnsNotFound()
    {
        // act
        var response = await _client.DeleteAsync($"/account/id/{12491938}");
        
        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    } 
    
    [Fact]
    public async Task Delete_NotValidId_ReturnsBadRequest()
    {
        // act
        var response = await _client.DeleteAsync($"/account/id/{"2 esdw"}");
        
        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    } 
}