using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

using Microsoft.EntityFrameworkCore;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.IntegrationTests
{
    public class AccountControllerGetByIdTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private HttpClient _client;

        public AccountControllerGetByIdTests()
        {
            CustomWebApplicationFactory<Program> factory = new CustomWebApplicationFactory<Program>();
            _client = factory.CreateClient();
        }

        [Theory]
        [InlineData("2137")]
        public async Task GetById_ValidIdForExistingAccount_ReturnsOk(string id)
        {
            // act
            var response = await _client.GetAsync($"/account/id/{id}");

            // assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }
        
        [Theory]
        [InlineData("0735")]
        [InlineData("32148193")]
        public async Task GetById_ValidIdForNotExistingAccount_ReturnsNotFound(string id)
        {
            // act
            var response = await _client.GetAsync($"/account/id/{id}");

            // assert
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }
        
        [Theory]
        [InlineData("wadasd")]
        [InlineData("212d7")]
        [InlineData("!")]
        public async Task GetById_NotValidId_ReturnsBadRequest(string id)
        {
            // act
            var response = await _client.GetAsync($"/account/id/{id}");

            // assert
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        } 
    }
}
