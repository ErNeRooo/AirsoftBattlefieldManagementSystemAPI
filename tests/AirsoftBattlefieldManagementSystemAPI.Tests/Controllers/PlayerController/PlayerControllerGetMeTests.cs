using System.Net;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Player;
using AirsoftBattlefieldManagementSystemAPI.Tests.Helpers;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Controllers.PlayerController;

public class PlayerControllerGetMeTests
{
    [Theory]
    [InlineData(1, "Chisato", false, 1, 1)]
    [InlineData(2, "Takina", false, 2, 1)]
    [InlineData(4, "Aqua", true, 3, 2)]
    [InlineData(5, "Nobody", true, null, null)]
    public async Task GetMe_ReturnsOkAndPlayerDto(int senderPlayerId, string name, bool isDead, int? teamId, int? roomId)
    {
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        var client = factory.CreateClient();
        
        var response = await client.GetAsync($"player/me");
        var result = await response.Content.DeserializeFromHttpContentAsync<PlayerDto>();
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        result.ShouldNotBeNull();
        result.PlayerId.ShouldBe(senderPlayerId);
        result.TeamId.ShouldBe(teamId);
        result.RoomId.ShouldBe(roomId);
        result.IsDead.ShouldBe(isDead);
        result.Name.ShouldBe(name);
    }
}