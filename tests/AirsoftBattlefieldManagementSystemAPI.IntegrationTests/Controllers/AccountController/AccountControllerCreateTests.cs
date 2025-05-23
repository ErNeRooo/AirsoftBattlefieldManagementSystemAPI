using System.Net;
using System.Text;
using AirsoftBattlefieldManagementSystemAPI.IntegrationTests.Helpers;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using Newtonsoft.Json;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.IntegrationTests;

public class AccountControllerCreateTests
{
    private HttpClient _client;

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
        var response = await _client.PostAsync($"/account", model.ToJsonHttpContent());

        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        response.Headers.Location.ShouldNotBeNull();
    }
    
    [Theory]
    [InlineData("test.com")]
    [InlineData("fafarafa")]
    [InlineData("@.")]
    [InlineData("@.com")]
    [InlineData("@test.")]
    [InlineData("")]
    public async Task Create_WrongEmailFormat_ReturnsBadRequest(string email)
    {
        // arrange
        var model = new PostAccountDto
        {
            Email = email,
            Password = "$troNg-P4SSw0rd"
        };
        
        // act
        var response = await _client.PostAsync($"/account", model.ToJsonHttpContent());

        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task Create_EmailIsMissing_ReturnsBadRequest()
    {
        // arrange
        var model = new PostAccountDto
        {
            Password = "$troNg-P4SSw0rd"
        };

        // act
        var response = await _client.PostAsync($"/account", model.ToJsonHttpContent());

        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
    
    [Theory]
    [InlineData("seededEmail1@test.com")]
    [InlineData("seededEmail2@test.com")]
    public async Task Create_EmailIsOccupied_ReturnsBadRequest(string email)
    {
        // arrange
        var model = new PostAccountDto
        {
            Email = email,
            Password = "$troNg-P4SSw0rd"
        };
        
        // act
        var response = await _client.PostAsync($"/account", model.ToJsonHttpContent());

        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task Create_PasswordIsMissing_ReturnsBadRequest()
    {
        // arrange
        var model = new PostAccountDto
        {
            Email = "dqgokkrjief@test.com",
        };

        // act
        var response = await _client.PostAsync($"/account", model.ToJsonHttpContent());

        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData("a")]
    [InlineData("aaaaaaaa")]
    [InlineData("aaaaaaaaa")]
    public async Task Create_PasswordIsTooShort_ReturnsBadRequest(string password)
    {
        // arrange
        var model = new PostAccountDto
        {
            Email = "ddfwfief@test.com",
            Password = password
        };

        // act
        var response = await _client.PostAsync($"/account", model.ToJsonHttpContent());

        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task Create_SecondAccountForTheSamePlayer_ReturnsConflictForSecondAttempt()
    {
        // arrange
        var firstModel = new PostAccountDto
        {
            Email = "first@test.com",
            Password = "second@test.com"
        };
        var secondModel = new PostAccountDto
        {
            Email = "second@test.com",
            Password = "second@test.com"
        };

        // act
        await _client.PostAsync($"/account", firstModel.ToJsonHttpContent());
        var response = await _client.PostAsync($"/account", secondModel.ToJsonHttpContent());
        
        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.Conflict);
    }
}