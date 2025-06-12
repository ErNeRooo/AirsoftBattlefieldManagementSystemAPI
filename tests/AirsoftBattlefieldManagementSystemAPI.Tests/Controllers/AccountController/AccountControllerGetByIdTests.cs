using System.Net;
using AirsoftBattlefieldManagementSystemAPI.Tests.Helpers;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Controllers.AccountController
{
    public class AccountControllerGetByIdTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private HttpClient _client;
        private readonly string _endpoint = "/account/id/";
        
        public AccountControllerGetByIdTests()
        {
            CustomWebApplicationFactory<Program> factory = new CustomWebApplicationFactory<Program>();
            _client = factory.CreateClient();
        }

        [Theory]
        [InlineData(2137, "seededEmail1@test.com", 2)]
        public async Task GetById_ValidIdForExistingAccount_ReturnsOkAndAccountDto(int accountId, string email, int playerId)
        {
            var response = await _client.GetAsync($"/account/id/{accountId}");
            var result = await response.Content.DeserializeFromHttpContentAsync<AccountDto>();
            
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            result.ShouldNotBeNull();
            result.AccountId.ShouldBe(accountId);
            result.Email.ShouldBe(email);
            result.PlayerId.ShouldBe(playerId);
        }
        
        [Theory]
        [InlineData("0735")]
        [InlineData("32148193")]
        public async Task GetById_ValidIdForNotExistingAccount_ReturnsNotFound(string id)
        {
            var response = await _client.GetAsync($"/account/id/{id}");
            
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }
    }
}
