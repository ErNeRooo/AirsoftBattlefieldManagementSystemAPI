using System.Net;
using AirsoftBattlefieldManagementSystemAPI.IntegrationTests.Helpers;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Account;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.IntegrationTests.Controllers.AccountController;

public class AccountControllerUpdateTests
{
    private HttpClient _client;

    public AccountControllerUpdateTests()
    {
        CustomWebApplicationFactory<Program> factory = new CustomWebApplicationFactory<Program>();
        _client = factory.CreateClient();
    }
    
    [Fact]
    public async Task Update_AllFieldsSpecified_ReturnsOk()
    {
        // arrange
        var model = new PutAccountDto()
        {
            Email = "seededEmaillll@gmail.com",
            Password = "$troNg-P4SSw0rd"
        };

        // act
        var response = await _client.PutAsync($"/account/id/{2137}", model.ToJsonHttpContent());
        
        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    } 
    
    [Fact]
    public async Task Update_OnlyEmailSpecified_ReturnsOk()
    {
        // arrange
        var model = new PutAccountDto()
        {
            Email = "seededEmail@gmail.com"
        };
        
        // act
        var response = await _client.PutAsync($"/account/id/{2137}", model.ToJsonHttpContent());
        
        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    } 
    
    [Fact]
    public async Task Update_OnlyPasswordSpecified_ReturnsOk()
    {
        // arrange
        var model = new PutAccountDto()
        {
            Password = "$troNg-P4SSw0rd"
        };
        
        // act
        var response = await _client.PutAsync($"/account/id/{2137}", model.ToJsonHttpContent());
        
        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    } 

    [Fact]
    public async Task Update_AccountDoesNotExist_ReturnsNotFound()
    {
        // arrange
        var model = new PutAccountDto()
        {
            Password = "$troNg-P4SSw0rd"
        };
        
        // act
        var response = await _client.PutAsync($"/account/id/{7472562}", model.ToJsonHttpContent());
        
        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    } 
}