using System.Net;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Zone;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.ZoneVertex;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Tests.Helpers;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Controllers.ZoneController;

public class ZoneControllerCreateTests
{
    private HttpClient _client;
    private string _endpoint = "zone/";

    public class ZoneTestData
    {
        public int SenderPlayerId { get; set; }
        public PostZoneDto Zone { get; set; }
    }

    public static IEnumerable<object[]> GetValidTestData()
    {
        var tests = new List<ZoneTestData>
        {
            new()
            {
                SenderPlayerId = 1,
                Zone = new PostZoneDto()
                {
                    BattleId = 1,
                    Name = "",
                    Type = ZoneTypes.SPAWN,
                    Vertices = new List<PostZoneVertexDto>
                    {
                        new (10, 10),
                        new (12, 10),
                        new (12, 11),
                    }
                }
            },
            new()
            {
                SenderPlayerId = 3,
                Zone = new PostZoneDto()
                {
                    BattleId = 2,
                    Name = "dawdsa",
                    Type = ZoneTypes.NO_FIRE_ZONE,
                    Vertices = new List<PostZoneVertexDto>
                    {
                        new (10, 10),
                        new (12, 10),
                        new (12, 11),
                    }
                }
            },
        };
        
        return tests.Select(test => new object[] { test });
    }
    
    [Theory]
    [MemberData(nameof(GetValidTestData))]
    public async void Create_Valid_ReturnsCreatedAndZoneDto(ZoneTestData testData)
    {
        // Arrange
        var factory = new CustomWebApplicationFactory<Program>(testData.SenderPlayerId);
        _client = factory.CreateClient();
        
        // Act
        var response = await _client.PostAsync($"{_endpoint}", testData.Zone.ToJsonHttpContent());
        ZoneDto? resultZone = await response.Content.DeserializeFromHttpContentAsync<ZoneDto>();
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        resultZone.ShouldNotBeNull();
        resultZone.BattleId.ShouldBe(testData.Zone.BattleId);
        resultZone.Name.ShouldBe(testData.Zone.Name);
        resultZone.Type.ShouldBe(testData.Zone.Type);
        
        for (int i = 0; i < resultZone.Vertices.Count; i++)
        {
            PostZoneVertexDto expectedVertex = testData.Zone.Vertices[i];
            ZoneVertexDto actualVertex = resultZone.Vertices[i];
            
            actualVertex.Longitude.ShouldBe(expectedVertex.Longitude);
            actualVertex.Latitude.ShouldBe(expectedVertex.Latitude);
        }
    }
    
    [Theory]
    [InlineData(2, 1)]
    [InlineData(4, 2)]
    [InlineData(9, 2)]
    [InlineData(12, 2)]
    public async void Create_PlayerIsNotAdmin_ReturnsForbidden(int senderPlayerId, int battleId)
    {
        // Arrange
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        PostZoneDto postZoneDto = new()
        {
            BattleId = battleId,
            Name = "No fire zone",
            Type = ZoneTypes.NO_FIRE_ZONE,
            Vertices = new List<PostZoneVertexDto>
            {
                new (10, 10),
                new (10, 10),
                new (10, 10),
            }
        };
        
        // Act
        var response = await _client.PostAsync($"{_endpoint}", postZoneDto.ToJsonHttpContent());
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
    
    [Theory]
    [InlineData(1, 2)]
    [InlineData(2, 2)]
    [InlineData(3, 1)]
    [InlineData(9, 1)]
    public async void Create_PlayerIsInDifferentRoom_ReturnsForbidden(int senderPlayerId, int battleId)
    {
        // Arrange
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        PostZoneDto postZoneDto = new()
        {
            BattleId = battleId,
            Name = "No fire zone",
            Type = ZoneTypes.NO_FIRE_ZONE,
            Vertices = new List<PostZoneVertexDto>
            {
                new (10, 10),
                new (10, 10),
                new (10, 10),
            }
        };
        
        // Act
        var response = await _client.PostAsync($"{_endpoint}", postZoneDto.ToJsonHttpContent());
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}