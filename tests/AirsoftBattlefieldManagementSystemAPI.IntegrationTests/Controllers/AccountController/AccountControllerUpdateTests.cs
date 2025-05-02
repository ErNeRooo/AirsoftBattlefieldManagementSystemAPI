using System.Net;
using System.Text;
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
    private BattleManagementSystemDbContext _server;
    
    public AccountControllerUpdateTests()
    {
        var r = new CustomWebApplicationFactory<Program>();
        //_server = r.Services.GetService<BattleManagementSystemDbContext>();
        _client = r.CreateClient();
    }

    [Fact]
    public async Task Update_AllFieldsSpecified_ReturnsOk()
    {
        string email = "seededEmaillll@gmail.com";
        string password = "StrongPassword";
        
        var model = new PutAccountDto()
        {
            Email = email,
            Password = password
        };

        var json = JsonConvert.SerializeObject(model);
        var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
        var id = 2137;

        var response = await _client.PutAsync($"/account/id/{id}", httpContent);

        var a = await response.Content.ReadAsStringAsync();
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    } 
    
    [Fact]
    public async Task Update_OnlyEmailSpecified_ReturnsOk()
    {
        string email = "seededEmail@gmail.com";
        
        var model = new PutAccountDto()
        {
            Email = email
        };

        var json = JsonConvert.SerializeObject(model);
        var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
        var id = 2137;
        
        var response = await _client.PutAsync($"/account/id/{id}", httpContent);
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    } 
    
    [Fact]
    public async Task Update_OnlyPasswordSpecified_ReturnsOk()
    {
        string password = "veryStrongPassword";
        
        var model = new PutAccountDto()
        {
            Password = password
        };

        var json = JsonConvert.SerializeObject(model);
        var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
        var id = 2137;
        
        var response = await _client.PutAsync($"/account/id/{id}", httpContent);
        
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
        var model = new PutAccountDto()
        {
            Email = email
        };

        var json = JsonConvert.SerializeObject(model);
        var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
        var id = 2137;
        
        var response = await _client.PutAsync($"/account/id/{id}", httpContent);
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    } 
    
    [Fact]
    public async Task Update_EmailIsOccupied_ReturnsBadRequest()
    {
        string email = "seededEmail2@test.com";
        
        var model = new PutAccountDto()
        {
            Email = email
        };

        var json = JsonConvert.SerializeObject(model);
        var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
        var id = 2137;
        
        var response = await _client.PutAsync($"/account/id/{id}", httpContent);
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    } 
    
    [Fact]
    public async Task Update_PasswordIsTooShort_ReturnsBadRequest()
    {
        string password = "123456789";
        
        var model = new PutAccountDto()
        {
            Password = password
        };

        var json = JsonConvert.SerializeObject(model);
        var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
        var id = 2137;
        
        var response = await _client.PutAsync($"/account/id/{id}", httpContent);
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    } 

    [Fact]
    public async Task Update_AccountDoNotExist_ReturnsNotFound()
    {
        string password = "veryStrongPassword";
        
        var model = new PutAccountDto()
        {
            Password = password
        };

        var json = JsonConvert.SerializeObject(model);
        var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
        var id = 7472562;
        
        var response = await _client.PutAsync($"/account/id/{id}", httpContent);
        
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    } 
}