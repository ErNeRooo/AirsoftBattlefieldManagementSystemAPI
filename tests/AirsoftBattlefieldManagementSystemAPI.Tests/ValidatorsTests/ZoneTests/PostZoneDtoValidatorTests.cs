using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Zone;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.ZoneVertex;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Models.Validators.Zone;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.ValidatorsTests.ZoneTests;

public class PostZoneDtoValidatorTests
{
    private BattleManagementSystemDbContext _dbContext;
    
    public PostZoneDtoValidatorTests()
    {
        var builder = new DbContextOptionsBuilder<BattleManagementSystemDbContext>();
        builder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        _dbContext = new BattleManagementSystemDbContext(builder.Options);
        Seed();
    }

    public void Seed()
    {
        _dbContext.Battle.Add(new Battle
        {
            BattleId = 1,
            IsActive = false,
            Name = "Kursk",
            RoomId = 1,
        });
        _dbContext.Battle.Add(new Battle
        {
            BattleId = 2,
            IsActive = true,
            Name = "Rhine",
            RoomId = 2,
        });
        
        _dbContext.ZoneVertex.Add(new ZoneVertex
        {
            ZoneVertexId = 1,
            Longitude = 10,
            Latitude = 10,
            ZoneId = 1
        });
        _dbContext.ZoneVertex.Add(new ZoneVertex
        {
            ZoneVertexId = 2,
            Longitude = 12,
            Latitude = 10,
            ZoneId = 1
        });
        _dbContext.ZoneVertex.Add(new ZoneVertex
        {
            ZoneVertexId = 3,
            Longitude = 12,
            Latitude = 11,
            ZoneId = 1
        });
        _dbContext.ZoneVertex.Add(new ZoneVertex
        {
            ZoneVertexId = 4,
            Longitude = 10,
            Latitude = 11,
            ZoneId = 1
        });
        
        _dbContext.Zone.Add(new Zone
        {
            ZoneId = 1,
            BattleId = 1,
            Name = "Test Zone",
            Type = ZoneTypes.NO_FIRE_ZONE
        });
        
        _dbContext.ZoneVertex.Add(new ZoneVertex
        {
            ZoneVertexId = 5,
            Longitude = 20.0m,
            Latitude = 20.0m,
            ZoneId = 2
        });
        _dbContext.ZoneVertex.Add(new ZoneVertex
        {
            ZoneVertexId = 6,
            Longitude = 23.0m,
            Latitude = 20.5m,
            ZoneId = 2
        });
        _dbContext.ZoneVertex.Add(new ZoneVertex
        {
            ZoneVertexId = 7,
            Longitude = 22.5m,
            Latitude = 23.0m,
            ZoneId = 2
        });
        _dbContext.ZoneVertex.Add(new ZoneVertex
        {
            ZoneVertexId = 8,
            Longitude = 19.5m,
            Latitude = 22.0m,
            ZoneId = 2
        });

        _dbContext.Zone.Add(new Zone
        {
            ZoneId = 2,
            BattleId = 2,
            Name = "Alpha Zone",
            Type = ZoneTypes.NO_FIRE_ZONE
        });
        
        _dbContext.ZoneVertex.Add(new ZoneVertex
        {
            ZoneVertexId = 9,
            Longitude = 30.0m,
            Latitude = 25.0m,
            ZoneId = 3
        });
        _dbContext.ZoneVertex.Add(new ZoneVertex
        {
            ZoneVertexId = 10,
            Longitude = 33.0m,
            Latitude = 26.0m,
            ZoneId = 3
        });
        _dbContext.ZoneVertex.Add(new ZoneVertex
        {
            ZoneVertexId = 11,
            Longitude = 31.5m,
            Latitude = 28.5m,
            ZoneId = 3
        });
        
        _dbContext.Zone.Add(new Zone
        {
            ZoneId = 3,
            BattleId = 2,
            Name = "Bravo Zone",
            Type = ZoneTypes.SPAWN
        });
        
        _dbContext.SaveChanges();
    }
    
    public static IEnumerable<object[]> GetValidZone()
    {
        var zones = new List<PostZoneDto>
        {
            new ()
            {
                Name = "Test Zone",
                Type = ZoneTypes.NO_FIRE_ZONE,
                BattleId = 1,
                Vertices = new List<PostZoneVertexDto>
                {
                    new (0,0),
                    new (0,0),
                    new (0,0),
                }
            },
            new ()
            {
                Name = "!@#$%^&*()-\\/[]{}();:'\"",
                Type = ZoneTypes.SPAWN,
                BattleId = 2,
                Vertices = new List<PostZoneVertexDto>
                {
                    new (0,0),
                    new (0,0),
                    new (0,0),
                    new (0,0),
                }
            },
            new ()
            {
                Name = "",
                Type = "SPAWN",
                BattleId = 2,
                Vertices = new List<PostZoneVertexDto>
                {
                    new (0,0), new (0,0),
                    new (0,0), new (0,0),
                    new (0,0), new (0,0),
                    new (0,0), new (0,0),
                    new (0,0), new (0,0),
                    new (0,0), new (0,0),
                    new (0,0), new (0,0),
                    new (0,0), new (0,0),
                    new (0,0), new (0,0),
                    new (0,0), new (0,0),
                }
            },
        };
        
        return zones.Select(x => new object[] { x });
    }
    
    public static IEnumerable<object[]> GetInvalidZone()
    {
        var zones = new List<PostZoneDto>
        {
            new ()
            {
                Name = "Test Zone",
                Type = "",
                BattleId = 1,
                Vertices = new List<PostZoneVertexDto>
                {
                    new (0,0),
                    new (0,0),
                    new (0,0),
                }
            },
            new ()
            {
                Name = "oWo",
                Type = "spawn",
                BattleId = 2,
                Vertices = new List<PostZoneVertexDto>
                {
                    new (0,0),
                    new (0,0),
                    new (0,0),
                    new (0,0),
                }
            },
            new ()
            {
                Name = "oWo",
                Type = "spawn",
                BattleId = 23,
                Vertices = new List<PostZoneVertexDto>
                {
                    new (0,0),
                    new (0,0),
                    new (0,0),
                    new (0,0),
                }
            },
            new ()
            {
                Name = "oWo",
                Type = "spawn",
                BattleId = 0,
                Vertices = new List<PostZoneVertexDto>
                {
                    new (0,0),
                    new (0,0),
                    new (0,0),
                    new (0,0),
                }
            },
            new ()
            {
                Name = "uWu",
                Type = ZoneTypes.NO_FIRE_ZONE,
                BattleId = 2,
                Vertices = new List<PostZoneVertexDto>
                {
                    new (0,0),
                    new (0,0),
                }
            },
            new ()
            {
                Name = "uWu",
                Type = ZoneTypes.NO_FIRE_ZONE,
                BattleId = 2,
                Vertices = new List<PostZoneVertexDto>()
            },
            new ()
            {
                Name = "",
                Type = ZoneTypes.SPAWN,
                BattleId = 2,
                Vertices = new List<PostZoneVertexDto>
                {
                    new (0,0), new (0,0),
                    new (0,0), new (0,0),
                    new (0,0), new (0,0),
                    new (0,0), new (0,0),
                    new (0,0), new (0,0),
                    new (0,0), new (0,0),
                    new (0,0), new (0,0),
                    new (0,0), new (0,0),
                    new (0,0), new (0,0),
                    new (0,0), new (0,0),
                    new (0,0),
                }
            },
            new ()
            {
                Name = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
                Type = "SPAWN",
                BattleId = 1,
                Vertices = new List<PostZoneVertexDto>
                {
                    new (0,0),
                    new (0,0),
                    new (0,0),
                    new (0,0),
                }
            }
        };
        
        return zones.Select(x => new object[] { x });
    }
    
    [Theory]
    [MemberData(nameof(GetValidZone))]
    public void Validate_ForValidModel_ReturnsSuccess(PostZoneDto model)
    {
        var validator = new PostZoneDtoValidator(_dbContext);

        var result = validator.TestValidate(model);
        
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Theory]
    [MemberData(nameof(GetInvalidZone))]
    public void Validate_ForInvalidModel_ReturnsFailure(PostZoneDto model)
    {
        var validator = new PostZoneDtoValidator(_dbContext);

        var result = validator.TestValidate(model);
        
        result.ShouldHaveAnyValidationError();
    }
}