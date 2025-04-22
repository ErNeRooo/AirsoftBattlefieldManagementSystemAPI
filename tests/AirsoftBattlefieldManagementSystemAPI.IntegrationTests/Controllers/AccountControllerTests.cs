using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

using Microsoft.EntityFrameworkCore;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;

namespace AirsoftBattlefieldManagementSystemAPI.IntegrationTests
{
    public class AccountControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private HttpClient _client;
        
        public AccountControllerTests()
        {
            _client = new CustomWebApplicationFactory<Program>().CreateClient();
        }
        
        [Theory]
        [InlineData("2137")]
        public async Task GetById_ValidId_ReturnsOkObjectResult(string id)
        {
            // act
            var response = await _client.GetAsync($"/account/id/{id}");

            // assert
            response.EnsureSuccessStatusCode();
        }
    }
}
