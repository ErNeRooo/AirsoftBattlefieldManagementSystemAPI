using System.Net;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.MapPing;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Zone;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.ZoneVertex;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Controllers.ZoneController;

public class ZoneControllerGetManyByBattleIdTests
{
    private HttpClient _client;
    private string _endpoint = "zone/";

    public class ZoneTestData
    {
        public int SenderPlayerId { get; set; }
        public int BattleId { get; set; }
        public List<ZoneDto> ExpectedZones { get; set; }
    }

    public static IEnumerable<object[]> GetTestData()
    {
        var tests = new List<ZoneTestData>
        {
            new()
            {
                SenderPlayerId = 1,
                BattleId = 1,
                ExpectedZones = new List<ZoneDto>
                {
                    new()
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
                }
            },
            new()
            {
                SenderPlayerId = 3,
                BattleId = 2,
                ExpectedZones = new List<ZoneDto>
                {
                    new()
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
                    },
                    new()
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
            },
            new()
            {
                SenderPlayerId = 12,
                BattleId = 2,
                ExpectedZones = new List<ZoneDto>
                {
                    new()
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
                    },
                    new()
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
            },
            new()
            {
                SenderPlayerId = 9,
                BattleId = 2,
                ExpectedZones = new List<ZoneDto>
                {
                    new()
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
                    },
                    new()
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
            },
            new()
            {
                SenderPlayerId = 10,
                BattleId = 2,
                ExpectedZones = new List<ZoneDto>
                {
                    new()
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
                    },
                    new()
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
            },
        };
        
        return tests.Select(test => new object[] { test });
    }
    
    [Theory]
    [MemberData(nameof(GetTestData))]
    public async void GetManyByBattleId_ValidId_ReturnsOkAndListOfZoneDtos(ZoneTestData testData)
    {
        // Arrange
        var factory = new CustomWebApplicationFactory<Program>(testData.SenderPlayerId);
        _client = factory.CreateClient();
        
        // Act
        var response = await _client.GetAsync($"{_endpoint}battleId/{testData.BattleId}");
        List<ZoneDto>? result = await response.Content.ReadFromJsonAsync<List<ZoneDto>>();
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        result.ShouldNotBeNull();

        for (int i = 0; i < testData.ExpectedZones.Count; i++)
        {
            ZoneDto expectedZone = testData.ExpectedZones[i];
            ZoneDto actualZone = result[i];
            
            actualZone.ZoneId.ShouldBe(expectedZone.ZoneId);
            actualZone.BattleId.ShouldBe(expectedZone.BattleId);
            actualZone.Name.ShouldBe(expectedZone.Name);
            actualZone.Type.ShouldBe(expectedZone.Type);

            for (int j = 0; j < actualZone.Vertices.Count; j++)
            {
                ZoneVertexDto expectedVertex = expectedZone.Vertices[j];
                ZoneVertexDto actualVertex = actualZone.Vertices[j];
            
                actualVertex.Longitude.ShouldBe(expectedVertex.Longitude);
                actualVertex.Latitude.ShouldBe(expectedVertex.Latitude);
            }
        }
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(234641)]
    [InlineData(-1)]
    public async void GetManyByBattleId_ForNotExistingBattle_ReturnsNotFound(int battleId)
    {
        // Arrange
        var factory = new CustomWebApplicationFactory<Program>();
        _client = factory.CreateClient();

        // Act
        var response = await _client.GetAsync($"{_endpoint}battleId/{battleId}");
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
    
    [Theory]
    [InlineData(1, 2)]
    [InlineData(2, 2)]
    [InlineData(3, 1)]
    [InlineData(4, 1)]
    [InlineData(9, 1)]
    public async void GetManyByBattleId_PlayerIsNotInTheSameRoom_ReturnsForbidden(int senderPlayerId, int battleId)
    {
        // Arrange
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        // Act
        var response = await _client.GetAsync($"{_endpoint}battleId/{battleId}");
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}