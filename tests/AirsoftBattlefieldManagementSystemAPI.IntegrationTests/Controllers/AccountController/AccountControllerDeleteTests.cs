using System.Net;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.IntegrationTests;

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
        var id = 2137;

        var response = await _client.DeleteAsync($"/account/id/{id}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    } 
    
    [Fact]
    public async Task Delete_AccountDoNotExist_ReturnsNotFound()
    {
        var id = 12491938;

        var response = await _client.DeleteAsync($"/account/id/{id}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    } 
    
    [Fact]
    public async Task Delete_NotValidId_ReturnsBadRequest()
    {
        var id = "2 esdw";

        var response = await _client.DeleteAsync($"/account/id/{id}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    } 
}