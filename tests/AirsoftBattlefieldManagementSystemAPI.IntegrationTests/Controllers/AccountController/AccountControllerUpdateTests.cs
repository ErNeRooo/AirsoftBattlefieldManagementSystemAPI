using System.Net;
using System.Text;
using AirsoftBattlefieldManagementSystemAPI.IntegrationTests.Helpers;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.IntegrationTests;

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
            Password = "StrongPassword"
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
            Password = "veryStrongPassword"
        };
        
        // act
        var response = await _client.PutAsync($"/account/id/{2137}", model.ToJsonHttpContent());
        
        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    } 
    
    [Theory]
    [InlineData("test.com")]
    [InlineData("fafarafa")]
    [InlineData("@.")]
    [InlineData("@.com")]
    [InlineData("@test.")]
    [InlineData("")]
    public async Task Update_WrongEmailFormat_ReturnsBadRequest(string email)
    {
        // arrange
        var model = new PutAccountDto()
        {
            Email = email
        };
        
        // act
        var response = await _client.PutAsync($"/account/id/{2137}", model.ToJsonHttpContent());
        
        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    } 
    
    [Fact]
    public async Task Update_EmailIsOccupied_ReturnsBadRequest()
    {
        // arrange
        var model = new PutAccountDto()
        {
            Email = "seededEmail2@test.com"
        };
        
        // act
        var response = await _client.PutAsync($"/account/id/{2137}", model.Email.ToJsonHttpContent());
        
        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    } 
    
    [Fact]
    public async Task Update_PasswordIsTooShort_ReturnsBadRequest()
    {
        // arrange
        var model = new PutAccountDto()
        {
            Password = "123456789"
        };
        
        // act
        var response = await _client.PutAsync($"/account/id/{2137}", model.ToJsonHttpContent());
        
        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    } 

    [Fact]
    public async Task Update_AccountDoesNotExist_ReturnsNotFound()
    {
        // arrange
        var model = new PutAccountDto()
        {
            Password = "veryStrongPassword"
        };
        
        // act
        var response = await _client.PutAsync($"/account/id/{7472562}", model.ToJsonHttpContent());
        
        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    } 
}