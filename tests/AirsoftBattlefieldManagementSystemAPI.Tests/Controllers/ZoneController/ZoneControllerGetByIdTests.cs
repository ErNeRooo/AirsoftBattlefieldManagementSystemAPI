using System.Net;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Zone;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.ZoneVertex;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Tests.Helpers;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Controllers.ZoneController;

public class ZoneControllerGetByIdTests
{
    private HttpClient _client;
    private string _endpoint = "zone/";

    public class ZoneTestData
    {
        public int SenderPlayerId { get; set; }
        public ZoneDto Zone { get; set; }
    }

    public static IEnumerable<object[]> GetTestData()
    {
        var tests = new List<ZoneTestData>
        {
            new()
            {
                SenderPlayerId = 1,
                Zone = new ZoneDto
                {
                    ZoneId = 1,
                    BattleId = 1,
                    Name = "Test Zone",
                    Type = ZoneTypes.NO_FIRE_ZONE,
                    Vertices = new List<ZoneVertexDto>
                    {
                        new (10, 10),
                        new (12, 10),
                        new (12, 11),
                        new (10, 11)
                    }
                }
            },
            new()
            {
                SenderPlayerId = 3,
                Zone = new ZoneDto
                {
                    ZoneId = 2,
                    BattleId = 2,
                    Name = "Alpha Zone",
                    Type = ZoneTypes.NO_FIRE_ZONE,
                    Vertices = new List<ZoneVertexDto>
                    {
                        new (20, 20),
                        new (23, 20.5m),
                        new (22.5m, 23),
                        new (19.5m, 22)
                    }
                }
            },
            new()
            {
                SenderPlayerId = 4,
                Zone = new ZoneDto
                {
                    ZoneId = 2,
                    BattleId = 2,
                    Name = "Alpha Zone",
                    Type = ZoneTypes.NO_FIRE_ZONE,
                    Vertices = new List<ZoneVertexDto>
                    {
                        new (20, 20),
                        new (23, 20.5m),
                        new (22.5m, 23),
                        new (19.5m, 22)
                    }
                }
            },
            new()
            {
                SenderPlayerId = 6,
                Zone = new ZoneDto
                {
                    ZoneId = 2,
                    BattleId = 2,
                    Name = "Alpha Zone",
                    Type = ZoneTypes.NO_FIRE_ZONE,
                    Vertices = new List<ZoneVertexDto>
                    {
                        new (20, 20),
                        new (23, 20.5m),
                        new (22.5m, 23),
                        new (19.5m, 22)
                    }
                }
            },
            new()
            {
                SenderPlayerId = 3,
                Zone = new ZoneDto
                {
                    ZoneId = 3,
                    BattleId = 2,
                    Name = "Bravo Zone",
                    Type = ZoneTypes.SPAWN,
                    Vertices = new List<ZoneVertexDto>
                    {
                        new (30, 25),
                        new (33, 26),
                        new (31.5m, 28.5m),
                    }
                }
            },
            new()
            {
                SenderPlayerId = 4,
                Zone = new ZoneDto
                {
                    ZoneId = 3,
                    BattleId = 2,
                    Name = "Bravo Zone",
                    Type = ZoneTypes.SPAWN,
                    Vertices = new List<ZoneVertexDto>
                    {
                        new (30, 25),
                        new (33, 26),
                        new (31.5m, 28.5m),
                    }
                }
            },
            new()
            {
                SenderPlayerId = 9,
                Zone = new ZoneDto
                {
                    ZoneId = 3,
                    BattleId = 2,
                    Name = "Bravo Zone",
                    Type = ZoneTypes.SPAWN,
                    Vertices = new List<ZoneVertexDto>
                    {
                        new (30, 25),
                        new (33, 26),
                        new (31.5m, 28.5m),
                    }
                }
            },
            new()
            {
                SenderPlayerId = 10,
                Zone = new ZoneDto
                {
                    ZoneId = 3,
                    BattleId = 2,
                    Name = "Bravo Zone",
                    Type = ZoneTypes.SPAWN,
                    Vertices = new List<ZoneVertexDto>
                    {
                        new (30, 25),
                        new (33, 26),
                        new (31.5m, 28.5m),
                    }
                }
            }
        };
        
        return tests.Select(test => new object[] { test });
    }
    
    [Theory]
    [MemberData(nameof(GetTestData))]
    public async void GetById_Valid_ReturnsOkAndZoneDto(ZoneTestData testData)
    {
        // Arrange
        var factory = new CustomWebApplicationFactory<Program>(testData.SenderPlayerId);
        _client = factory.CreateClient();
        
        // Act
        var response = await _client.GetAsync($"{_endpoint}id/{testData.Zone.ZoneId}");
        ZoneDto? resultZone = await response.Content.DeserializeFromHttpContentAsync<ZoneDto>();
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        resultZone.ShouldNotBeNull();
        resultZone.ZoneId.ShouldBe(testData.Zone.ZoneId);
        resultZone.Name.ShouldBe(testData.Zone.Name);
        resultZone.Type.ShouldBe(testData.Zone.Type);
        resultZone.BattleId.ShouldBe(testData.Zone.BattleId);

        for (int i = 0; i < resultZone.Vertices.Count; i++)
        {
            ZoneVertexDto expectedVertex = testData.Zone.Vertices[i];
            ZoneVertexDto actualVertex = resultZone.Vertices[i];
            
            actualVertex.Longitude.ShouldBe(expectedVertex.Longitude);
            actualVertex.Latitude.ShouldBe(expectedVertex.Latitude);
        }
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(234641)]
    [InlineData(-1)]
    public async void GetById_ForNotExistingZone_ReturnsNotFound(int id)
    {
        // Arrange
        var factory = new CustomWebApplicationFactory<Program>();
        _client = factory.CreateClient();

        // Act
        var response = await _client.GetAsync($"{_endpoint}id/{id}");
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
    
    [Theory]
    [InlineData(1, 2)]
    [InlineData(2, 3)]
    [InlineData(3, 1)]
    [InlineData(9, 1)]
    [InlineData(10, 1)]
    public async void GetById_PlayerIsNotInTheSameRoom_ReturnsForbidden(int senderPlayerId, int zoneId)
    {
        // Arrange
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        // Act
        var response = await _client.GetAsync($"{_endpoint}id/{zoneId}");
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}