using System.Net;
using AirsoftBattlefieldManagementSystemAPI.Tests.Helpers;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Account;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Controllers.AccountController;

public class AccountControllerUpdateTests
{
    private HttpClient _client;
    private readonly string _endpoint = "account";

    public AccountControllerUpdateTests()
    {
        var factory = new CustomWebApplicationFactory<Program>();
        _client = factory.CreateClient();
    }
    
    [Theory]
    [InlineData("seededEmaillll@gmail.com", "$troNg-P4SSw0rd")]
    public async Task Update_AllFieldsSpecified_ReturnsOkAndAccountDto(string email, string password)
    {
        var factory = new CustomWebApplicationFactory<Program>(2);
        _client = factory.CreateClient();
        
        var model = new PutAccountDto()
        {
            Email = email,
            Password = password
        };
        
        var response = await _client.PutAsync($"{_endpoint}", model.ToJsonHttpContent());
        var result = await response.Content.DeserializeFromHttpContentAsync<AccountDto>();
        
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
        var factory = new CustomWebApplicationFactory<Program>(2);
        _client = factory.CreateClient();
        
        var model = new PutAccountDto()
        {
            Email = email
        };
        
        var response = await _client.PutAsync($"{_endpoint}", model.ToJsonHttpContent());
        var result = await response.Content.DeserializeFromHttpContentAsync<AccountDto>();
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        result.ShouldNotBeNull();
        result.Email.ShouldBe(email);
        result.PlayerId.ShouldBe(2);
        result.AccountId.ShouldBe(2137);
    } 
    
    [Fact]
    public async Task Update_OnlyPasswordSpecified_ReturnsOkAndAccountDto()
    {
        var factory = new CustomWebApplicationFactory<Program>(2);
        _client = factory.CreateClient();
        
        var model = new PutAccountDto()
        {
            Password = "$troNg-P4SSw0rd"
        };
        
        var response = await _client.PutAsync($"{_endpoint}", model.ToJsonHttpContent());
        var result = await response.Content.DeserializeFromHttpContentAsync<AccountDto>();
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        result.ShouldNotBeNull();
        result.Email.ShouldBe("seededEmail1@test.com");
        result.PlayerId.ShouldBe(2);
        result.AccountId.ShouldBe(2137);
    } 
    
    [Theory]
    [InlineData(5)]
    public async Task Update_PlayerWithoutAccount_ReturnsNotFound(int senderPlayerId)
    {
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        var model = new PutAccountDto();
        
        var response = await _client.PutAsync($"{_endpoint}", model.ToJsonHttpContent());
        
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}