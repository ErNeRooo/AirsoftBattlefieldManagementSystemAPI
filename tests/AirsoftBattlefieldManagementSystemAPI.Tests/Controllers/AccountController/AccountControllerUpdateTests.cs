using System.Net;
using AirsoftBattlefieldManagementSystemAPI.Tests.Helpers;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Account;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Controllers.AccountController;

public class AccountControllerUpdateTests
{
    private HttpClient _client;
    private readonly string _endpoint = "/account/id/";

    public AccountControllerUpdateTests()
    {
        var factory = new CustomWebApplicationFactory<Program>();
        _client = factory.CreateClient();
    }
    
    [Theory]
    [InlineData("seededEmaillll@gmail.com", "$troNg-P4SSw0rd")]
    public async Task Update_AllFieldsSpecified_ReturnsOkAndAccountDto(string email, string password)
    {
        // arrange
        var factory = new CustomWebApplicationFactory<Program>(2);
        _client = factory.CreateClient();
        
        var model = new PutAccountDto()
        {
            Email = email,
            Password = password
        };

        // act
        var response = await _client.PutAsync($"{_endpoint}{2137}", model.ToJsonHttpContent());
        var result = await response.Content.DeserializeFromHttpContentAsync<AccountDto>();
        
        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        result.ShouldNotBeNull();
        result.Email.ShouldBe(email);
        result.PlayerId.ShouldBe(2);
        result.AccountId.ShouldBe(2137);
    } 
    
    [Theory]
    [InlineData("New.Email@gmail.com")]
    public async Task Update_OnlyEmailSpecified_ReturnsOkAndAccountDto(string email)
    {
        // arrange
        var factory = new CustomWebApplicationFactory<Program>(2);
        _client = factory.CreateClient();
        
        var model = new PutAccountDto()
        {
            Email = email
        };
        
        // act
        var response = await _client.PutAsync($"{_endpoint}{2137}", model.ToJsonHttpContent());
        var result = await response.Content.DeserializeFromHttpContentAsync<AccountDto>();
        
        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        result.ShouldNotBeNull();
        result.Email.ShouldBe(email);
        result.PlayerId.ShouldBe(2);
        result.AccountId.ShouldBe(2137);
    } 
    
    [Fact]
    public async Task Update_OnlyPasswordSpecified_ReturnsOkAndAccountDto()
    {
        // arrange
        var factory = new CustomWebApplicationFactory<Program>(2);
        _client = factory.CreateClient();
        
        var model = new PutAccountDto()
        {
            Password = "$troNg-P4SSw0rd"
        };
        
        // act
        var response = await _client.PutAsync($"{_endpoint}{2137}", model.ToJsonHttpContent());
        var result = await response.Content.DeserializeFromHttpContentAsync<AccountDto>();
        
        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        result.ShouldNotBeNull();
        result.Email.ShouldBe("seededEmail1@test.com");
        result.PlayerId.ShouldBe(2);
        result.AccountId.ShouldBe(2137);
    } 
    
    [Theory]
    [InlineData(1, 1)]
    [InlineData(1, 2)]
    [InlineData(2, 1)]
    [InlineData(2, 2)]
    public async Task Update_AccountOfOtherPlayer_ReturnsForbidden(int accountId, int playerId)
    {
        // arrange
        var factory = new CustomWebApplicationFactory<Program>(playerId);
        _client = factory.CreateClient();

        var model = new PutAccountDto();
        
        // act
        var response = await _client.PutAsync($"{_endpoint}{accountId}", model.ToJsonHttpContent());
        
        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
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
        var response = await _client.PutAsync($"{_endpoint}{7472562}", model.ToJsonHttpContent());
        
        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    } 
}