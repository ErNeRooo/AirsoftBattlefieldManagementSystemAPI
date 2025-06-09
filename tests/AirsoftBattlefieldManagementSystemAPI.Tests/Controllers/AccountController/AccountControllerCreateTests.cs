using System.Net;
using System.Text.Json;
using AirsoftBattlefieldManagementSystemAPI.Tests.Helpers;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Account;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Controllers.AccountController;

public class AccountControllerCreateTests
{
    private readonly HttpClient _client;
    private readonly string _endpoint = "account/signup";

    public AccountControllerCreateTests()
    {
        CustomWebApplicationFactory<Program> factory = new CustomWebApplicationFactory<Program>();
        _client = factory.CreateClient();
    }
    
    [Theory]
    [InlineData("dqsf2d.e3ed3@test.com", "$troNg-P4SSw0rd")]
    public async Task Create_ValidJson_ReturnsCreatedAndAccountDto(string email, string password)
    {
        // arrange
        var model = new PostAccountDto
        {
            Email = email,
            Password = password
        };

        // act
        var response = await _client.PostAsync(_endpoint, model.ToJsonHttpContent());
        var result = await response.Content.DeserializeFromHttpContentAsync<AccountDto>();
        
        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        response.Headers.Location.ShouldNotBeNull();
        result.ShouldNotBeNull();
        result.Email.ShouldBe("dqsf2d.e3ed3@test.com");
        result.AccountId.ShouldNotBe(0);
        result.PlayerId.ShouldBe(1);
    }
    
    [Fact]
    public async Task Create_SecondAccountForTheSamePlayer_ReturnsConflictForSecondAttempt()
    {
        // arrange
        var firstModel = new PostAccountDto
        {
            Email = "first@test.com",
            Password = "$troNg-P4SSw0rd"
        };
        var secondModel = new PostAccountDto
        {
            Email = "second@test.com",
            Password = "$troNg-P4SSw0rd"
        };

        // act
        await _client.PostAsync(_endpoint, firstModel.ToJsonHttpContent());
        var response = await _client.PostAsync(_endpoint, secondModel.ToJsonHttpContent());
        
        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.Conflict);
    }
}