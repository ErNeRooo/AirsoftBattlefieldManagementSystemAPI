using System.Net;
using AirsoftBattlefieldManagementSystemAPI.Tests.Helpers;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Account;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Controllers.AccountController;

public class AccountControllerUpdateTests
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public AccountControllerUpdateTests()
    {
        CustomWebApplicationFactory<Program> factory = new CustomWebApplicationFactory<Program>();
        _factory = factory;
        _client = factory.CreateClient();
    }
    
    [Theory]
    [InlineData("seededEmaillll@gmail.com", "$troNg-P4SSw0rd")]
    public async Task Update_AllFieldsSpecified_ReturnsOkAndAccountDto(string email, string password)
    {
        // arrange
        var model = new PutAccountDto()
        {
            Email = email,
            Password = password
        };

        // act
        var response = await _client.PutAsync($"/account/id/{2137}", model.ToJsonHttpContent());
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
        var model = new PutAccountDto()
        {
            Email = email
        };
        
        // act
        var response = await _client.PutAsync($"/account/id/{2137}", model.ToJsonHttpContent());
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
        var model = new PutAccountDto()
        {
            Password = "$troNg-P4SSw0rd"
        };
        
        // act
        var response = await _client.PutAsync($"/account/id/{2137}", model.ToJsonHttpContent());
        var result = await response.Content.DeserializeFromHttpContentAsync<AccountDto>();
        
        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        result.ShouldNotBeNull();
        result.Email.ShouldBe("seededEmail1@test.com");
        result.PlayerId.ShouldBe(2);
        result.AccountId.ShouldBe(2137);
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