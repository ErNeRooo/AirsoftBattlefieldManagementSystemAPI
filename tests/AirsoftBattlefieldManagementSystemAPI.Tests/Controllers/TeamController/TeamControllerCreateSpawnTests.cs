using System.Net;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Team;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Zone;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.ZoneVertex;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Tests.Helpers;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Controllers.TeamController;

public class TeamControllerCreateSpawnTests
{
    private HttpClient _client;
    private string _endpoint = "team/spawn/teamId/";
    
    public class ZoneTestData
    {
        public int SenderPlayerId { get; set; }
        public int TeamId { get; set; }
        public PostZoneDto Zone { get; set; }
    }
    
    public static IEnumerable<object[]> GetValidTestData()
    {
        var tests = new List<ZoneTestData>
        {
            new()
            {
                SenderPlayerId = 1,
                TeamId = 2,
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
                TeamId = 3,
                Zone = new PostZoneDto()
                {
                    BattleId = 2,
                    Name = "dawdsa",
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
                TeamId = 6,
                Zone = new PostZoneDto()
                {
                    BattleId = 2,
                    Name = "dawdsa",
                    Type = ZoneTypes.SPAWN,
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
    public async void CreateSpawn_Valid_ReturnsCreatedAndTeamDto(ZoneTestData testData)
    {
        // Arrange
        var factory = new CustomWebApplicationFactory<Program>(testData.SenderPlayerId);
        _client = factory.CreateClient();
        
        // Act
        var response = await _client.PostAsync($"{_endpoint}{testData.TeamId}", testData.Zone.ToJsonHttpContent());
        TeamDto? resultTeam = await response.Content.DeserializeFromHttpContentAsync<TeamDto>();
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        resultTeam.ShouldNotBeNull();
        resultTeam.SpawnZoneId.ShouldNotBe(0);
        resultTeam.SpawnZone?.BattleId.ShouldBe(testData.Zone.BattleId);
        resultTeam.SpawnZone?.Name.ShouldBe(testData.Zone.Name);
        resultTeam.SpawnZone?.Type.ShouldBe(testData.Zone.Type);
        
        for (int i = 0; i < resultTeam.SpawnZone?.Vertices.Count; i++)
        {
            PostZoneVertexDto expectedVertex = testData.Zone.Vertices[i];
            ZoneVertexDto actualVertex = resultTeam.SpawnZone?.Vertices[i];
            
            actualVertex.Longitude.ShouldBe(expectedVertex.Longitude);
            actualVertex.Latitude.ShouldBe(expectedVertex.Latitude);
        }
    }
    
    [Theory]
    [InlineData(2, 1, 2)]
    [InlineData(4, 2, 3)]
    [InlineData(9, 2, 6)]
    [InlineData(12, 2, 4)]
    public async void CreateSpawn_PlayerIsNotAdmin_ReturnsForbidden(int senderPlayerId, int battleId, int teamId)
    {
        // Arrange
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        PostZoneDto postZoneDto = new()
        {
            BattleId = battleId,
            Name = "No fire zone",
            Type = ZoneTypes.SPAWN,
            Vertices = new List<PostZoneVertexDto>
            {
                new (10, 10),
                new (10, 10),
                new (10, 10),
            }
        };
        
        // Act
        var response = await _client.PostAsync($"{_endpoint}{teamId}", postZoneDto.ToJsonHttpContent());
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(34214)]
    public async void DeleteSpawn_NotExistingTeam_ReturnsNotFound(int teamId)
    {
        var factory = new CustomWebApplicationFactory<Program>();
        _client = factory.CreateClient();

        PostZoneDto postZoneDto = new()
        {
            BattleId = 1,
            Name = "a",
            Type = ZoneTypes.SPAWN,
            Vertices = new List<PostZoneVertexDto>
            {
                new(10, 10),
                new(10, 10),
                new(10, 10),
            }
        };
        
        var response = await _client.PostAsync($"{_endpoint}{teamId}", postZoneDto.ToJsonHttpContent());
        
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
    
    [Theory]
    [InlineData(1, 2, 3)]
    [InlineData(1, 2, 1)]
    [InlineData(1, 1, 3)]
    [InlineData(2, 2, 3)]
    [InlineData(3, 1, 1)]
    [InlineData(9, 1, 6)]
    public async void CreateSpawn_PlayerIsInDifferentRoom_ReturnsForbidden(int senderPlayerId, int battleId, int teamId)
    {
        // Arrange
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        PostZoneDto postZoneDto = new()
        {
            BattleId = battleId,
            Name = "No fire zone",
            Type = ZoneTypes.SPAWN,
            Vertices = new List<PostZoneVertexDto>
            {
                new (10, 10),
                new (10, 10),
                new (10, 10),
            }
        };
        
        // Act
        var response = await _client.PostAsync($"{_endpoint}{teamId}", postZoneDto.ToJsonHttpContent());
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}