using System.Net;
using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Zone;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.ZoneVertex;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Tests.Helpers;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Controllers.ZoneController;

public class ZoneControllerUpdateTests
{
    private HttpClient _client;
    private string _endpoint = "zone/";

    [Theory]
    [InlineData(1, 1, "A", ZoneTypes.SPAWN)]
    [InlineData(3, 3, "dawdasdawd", ZoneTypes.NO_FIRE_ZONE)]
    public async void Update_Valid_ReturnsOkAndZoneDto(int senderPlayerId, int zoneId, string name, string type)
    {
        // Arrange
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();

        PutZoneDto putZoneDto = new()
        {
            Name = name,
            Type = type
        };

        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BattleManagementSystemDbContext>();

        Zone zoneBeforeUpdate = dbContext.Zone
            .Include(zone => zone.Vertices)
            .First(zone => zone.ZoneId == zoneId);
        
        // Act
        var response = await _client.PutAsync($"{_endpoint}id/{zoneId}", putZoneDto.ToJsonHttpContent());
        ZoneDto? resultZone = await response.Content.DeserializeFromHttpContentAsync<ZoneDto>();
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        resultZone.ShouldNotBeNull();
        resultZone.ZoneId.ShouldBe(zoneId);
        resultZone.Name.ShouldBe(name);
        resultZone.Type.ShouldBe(type);
        
        for (int i = 0; i < resultZone.Vertices.Count; i++)
        {
            ZoneVertex expectedVertex = zoneBeforeUpdate.Vertices[i];
            ZoneVertexDto actualVertex = resultZone.Vertices[i];
            
            actualVertex.Longitude.ShouldBe(expectedVertex.Longitude);
            actualVertex.Latitude.ShouldBe(expectedVertex.Latitude);
        }
    }
    
    [Theory]
    [InlineData(1, 1, null, ZoneTypes.NO_FIRE_ZONE)]
    [InlineData(3, 2, null, null)]
    [InlineData(3, 3, "adsw", null)]
    public async void Update_ValidButNotAllFieldsSpecified_ReturnsOkAndZoneDto(int senderPlayerId, int zoneId, string? name, string? type)
    {
        // Arrange
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();

        PutZoneDto putZoneDto = new()
        {
            Name = name,
            Type = type
        };

        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BattleManagementSystemDbContext>();

        Zone zoneBeforeUpdate = dbContext.Zone
            .Include(zone => zone.Vertices)
            .First(zone => zone.ZoneId == zoneId);
        
        // Act
        var response = await _client.PutAsync($"{_endpoint}id/{zoneId}", putZoneDto.ToJsonHttpContent());
        ZoneDto? resultZone = await response.Content.DeserializeFromHttpContentAsync<ZoneDto>();
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        resultZone.ShouldNotBeNull();
        resultZone.ZoneId.ShouldBe(zoneId);
        resultZone.Name.ShouldBe(name ?? zoneBeforeUpdate.Name);
        resultZone.Type.ShouldBe(type ?? zoneBeforeUpdate.Type);
        
        for (int i = 0; i < resultZone.Vertices.Count; i++)
        {
            ZoneVertex expectedVertex = zoneBeforeUpdate.Vertices[i];
            ZoneVertexDto actualVertex = resultZone.Vertices[i];
            
            actualVertex.Longitude.ShouldBe(expectedVertex.Longitude);
            actualVertex.Latitude.ShouldBe(expectedVertex.Latitude);
        }
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(234641)]
    [InlineData(-1)]
    public async void Update_ForNotExistingZone_ReturnsNotFound(int id)
    {
        // Arrange
        var factory = new CustomWebApplicationFactory<Program>();
        _client = factory.CreateClient();
        
        PutZoneDto putZoneDto = new();
        
        // Act
        var response = await _client.PutAsync($"{_endpoint}id/{id}", putZoneDto.ToJsonHttpContent());
        var message = await response.Content.ReadAsStringAsync();
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
    
    [Theory]
    [InlineData(1, 2)]
    [InlineData(1, 3)]
    [InlineData(3, 1)]
    [InlineData(2, 2)]
    [InlineData(9, 1)]
    [InlineData(10, 1)]
    [InlineData(12, 1)]
    public async void Update_PlayerIsNotInTheSameRoom_ReturnsForbidden(int senderPlayerId, int zoneId)
    {
        // Arrange
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();
        
        PutZoneDto putZoneDto = new();
        
        // Act
        var response = await _client.PutAsync($"{_endpoint}id/{zoneId}", putZoneDto.ToJsonHttpContent());
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
    
    [Theory]
    [InlineData(2, 1)]
    [InlineData(4, 2)]
    [InlineData(9, 2)]
    [InlineData(6, 3)]
    [InlineData(10, 3)]
    [InlineData(12, 3)]
    public async void Update_PlayerIsNotRoomAdmin_ReturnsForbidden(int senderPlayerId, int zoneId)
    {
        // Arrange
        var factory = new CustomWebApplicationFactory<Program>(senderPlayerId);
        _client = factory.CreateClient();

        PutZoneDto putZoneDto = new();
        
        // Act
        var response = await _client.PutAsync($"{_endpoint}id/{zoneId}", putZoneDto.ToJsonHttpContent());
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}