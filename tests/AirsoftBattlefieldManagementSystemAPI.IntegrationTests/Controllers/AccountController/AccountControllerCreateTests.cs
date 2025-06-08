using System.Net;
using AirsoftBattlefieldManagementSystemAPI.IntegrationTests.Helpers;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Account;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.IntegrationTests.Controllers.AccountController;

public class AccountControllerCreateTests
{
    private HttpClient _client;
    private string _endpoint = "account/signup";

    public AccountControllerCreateTests()
    {
        CustomWebApplicationFactory<Program> factory = new CustomWebApplicationFactory<Program>();
        _client = factory.CreateClient();
    }
    
    [Fact]
    public async Task Create_ValidJson_ReturnsCreated()
    {
        // arrange
        var model = new PostAccountDto
        {
            Email = "dqsf2de3ed3@test.com",
            Password = "$troNg-P4SSw0rd"
        };

        // act
        var response = await _client.PostAsync(_endpoint, model.ToJsonHttpContent());

        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        response.Headers.Location.ShouldNotBeNull();
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