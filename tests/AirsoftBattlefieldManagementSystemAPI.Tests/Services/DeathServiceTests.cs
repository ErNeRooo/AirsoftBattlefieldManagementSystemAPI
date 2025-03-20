using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Services.Abstractions;
using AirsoftBattlefieldManagementSystemAPI.Services.Implementations;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Services
{
    public class DeathServiceTests
    {
        private readonly DeathService _deathService;
        private readonly Mock<IBattleManagementSystemDbContext> _dbContext;
        private readonly Mock<IMapper> _mapper;
        private readonly Dictionary<Death, DeathDto> _deathsToDtos;
        private readonly List<Location> _locations;
        private readonly List<Player> _players;

        public DeathServiceTests()
        {
            _mapper = new Mock<IMapper>();
            _dbContext = new Mock<IBattleManagementSystemDbContext>();
            _deathService = new DeathService(_mapper.Object, _dbContext.Object);

            _deathsToDtos = new Dictionary<Death, DeathDto>()
            {
                {
                    new Death() { DeathId = 1, PlayerId = 4, LocationId = 2},
                    new DeathDto() { DeathId = 1, PlayerId = 4, LocationId = 2, Longitude = 64, Latitude = 21, Accuracy = 10, Bearing = 124, Time = new DateTime(1939, 9, 1, 5, 40, 0)}
                },
            };

            _locations = new List<Location>(){
                new Location{ LocationId = 1,  Longitude = 12, Latitude = 86, Accuracy = 15, Bearing = 180, Time = new DateTime(2025, 3, 11, 21, 37, 0) },
                new Location{ LocationId = 2,  Longitude = 64, Latitude = 21, Accuracy = 10, Bearing = 124, Time = new DateTime(1939, 9, 1, 5, 40, 0) }
            };

            _players = new List<Player>()
            {
                new Player { PlayerId = 4 }
            };

            _mapper.Setup(m => m.Map<Location>(It.IsAny<PostDeathDto>())).Returns(
                (PostDeathDto d) => new Location
                {
                    Longitude = d.Longitude,
                    Latitude = d.Latitude,
                    Accuracy = d.Accuracy,
                    Bearing = d.Bearing,
                    Time = d.Time
                });

            _mapper.Setup(m => m.Map<PutDeathDto, Location>(It.IsAny<PutDeathDto>(), It.IsAny<Location>())).Returns(
                (PutDeathDto src, Location dest) =>
                {
                    dest = new Location
                    {
                        Longitude = src.Longitude,
                        Latitude = src.Latitude,
                        Accuracy = src.Accuracy,
                        Bearing = src.Bearing,
                        Time = src.Time
                    };

                    return dest;
                });

            _mapper.Setup(m => m.Map<DeathDto>(It.IsAny<Death>())).Returns(
                (Death d) => {
                    var location = _locations.FirstOrDefault(
                        l => l.LocationId == d.LocationId
                    );

                    return new DeathDto
                    {
                        DeathId = d.DeathId,
                        LocationId = d.LocationId,
                        PlayerId = d.PlayerId,
                        Longitude = location.Longitude,
                        Latitude = location.Latitude,
                        Accuracy = location.Accuracy,
                        Bearing = location.Bearing,
                        Time = location.Time
                    };
                });
        }

        private DbSet<T> GetDbSet<T>(IQueryable<T> genericQueryable) where T : class
        {
            var dbSetMock = new Mock<DbSet<T>>();
            var genericMock = dbSetMock.As<IQueryable<T>>();

            genericMock.Setup(m => m.Provider).Returns(genericQueryable.Provider);
            genericMock.Setup(m => m.Expression).Returns(genericQueryable.Expression);
            genericMock.Setup(m => m.ElementType).Returns(genericQueryable.ElementType);
            genericMock.Setup(m => m.GetEnumerator()).Returns(genericQueryable.GetEnumerator());

            return dbSetMock.Object;
        }
        public static IEnumerable<object[]> GetDeathTestData()
        {
            yield return new object[] { new DeathDto() { DeathId = 1, PlayerId = 4, LocationId = 2, Longitude = 64, Latitude = 21, Accuracy = 10, Bearing = 124, Time = new DateTime(1939, 9, 1, 5, 40, 0) } };
        }

        [Theory]
        [MemberData(nameof(GetDeathTestData))]
        public void GetById_ForExistingDeathId_ReturnsDeathDto(DeathDto expectedDeath)
        {
            // arrange
            IQueryable<Death> deaths = _deathsToDtos.Keys.AsQueryable();
            DbSet<Death> deathDbSet = GetDbSet(deaths);
            DbSet<Location> locationDbSet = GetDbSet(_locations.AsQueryable());

            _dbContext.Setup(m => m.Location).Returns(locationDbSet);
            _dbContext.Setup(m => m.Death).Returns(deathDbSet);

            // act
            DeathDto result = _deathService.GetById(expectedDeath.DeathId);

            // assert
            result.ShouldSatisfyAllConditions(
                result => result.DeathId.ShouldBe(expectedDeath.DeathId),
                result => result.PlayerId.ShouldBe(expectedDeath.PlayerId),
                result => result.LocationId.ShouldBe(expectedDeath.LocationId),
                result => result.Longitude.ShouldBe(expectedDeath.Longitude),
                result => result.Latitude.ShouldBe(expectedDeath.Latitude),
                result => result.Accuracy.ShouldBe(expectedDeath.Accuracy),
                result => result.Bearing.ShouldBe(expectedDeath.Bearing),
                result => result.Time.ShouldBe(expectedDeath.Time));
        }

        [Theory]
        [InlineData(4)]
        public void GetAllOfPlayerWithId_ForExistingId_ReturnsNull(int id)
        {
            // arrange
            IQueryable<Death> deaths = _deathsToDtos.Keys.AsQueryable();
            DbSet<Death> deathDbSet = GetDbSet(deaths);
            DbSet<Location> locationDbSet = GetDbSet(_locations.AsQueryable());

            _dbContext.Setup(m => m.Location).Returns(locationDbSet);
            _dbContext.Setup(m => m.Death).Returns(deathDbSet);


            // act
            List<DeathDto> result = _deathService.GetAllOfPlayerWithId(id);

            // assert
            result.ShouldSatisfyAllConditions(
                result => result.Count.ShouldBe(1));
        }


        [Fact]
        public void Create_ForExistingPlayerId_ReturnsIdOfCreatedDeath()
        {
            // arrange
            List<Death> deaths = _deathsToDtos.Keys.ToList();
            DbSet<Death> deathDbSet = GetDbSet(deaths.AsQueryable()); 
            DbSet<Player> playerDbSet = GetDbSet(_players.AsQueryable());
            DbSet<Location> locationDbSet = GetDbSet(_locations.AsQueryable());

            _dbContext.Setup(m => m.Location).Returns(locationDbSet);
            _dbContext.Setup(m => m.Player).Returns(playerDbSet);
            _dbContext.Setup(m => m.Death).Returns(deathDbSet);
            _dbContext.Setup(m => m.Death.Add(It.IsAny<Death>())).Callback(
                (Death death) =>
                {
                    death.DeathId = deathDbSet.Count() + 1;
                    deaths.Add(death);
                });

            // act 
            int? result = _deathService.Create(4, new PostDeathDto());

            // assert
            result.ShouldBe(deathDbSet.Count());
        }

        [Fact]
        public void Create_ShouldCallAddMethodForDeathOnce()
        {
            // arrange
            List<Death> deaths = _deathsToDtos.Keys.ToList();
            DbSet<Death> dbSet = GetDbSet(deaths.AsQueryable());
            DbSet<Player> playerDbSet = GetDbSet(_players.AsQueryable());
            DbSet<Location> locationDbSet = GetDbSet(_locations.AsQueryable());

            _dbContext.Setup(m => m.Location).Returns(locationDbSet);
            _dbContext.Setup(m => m.Player).Returns(playerDbSet);
            _dbContext.Setup(m => m.Death).Returns(dbSet);

            // act
            _deathService.Create(4, new PostDeathDto());

            // assert
            _dbContext.Verify(m => m.Death.Add(It.IsAny<Death>()), Times.Once);
        }

        [Fact]
        public void Create_ShouldCallAddMethodForPlayerDeathOnce()
        {
            // arrange
            List<Death> deaths = _deathsToDtos.Keys.ToList();
            DbSet<Death> dbSet = GetDbSet(deaths.AsQueryable());
            DbSet<Player> playerDbSet = GetDbSet(_players.AsQueryable());
            DbSet<Location> locationDbSet = GetDbSet(_locations.AsQueryable());

            _dbContext.Setup(m => m.Location).Returns(locationDbSet);
            _dbContext.Setup(m => m.Player).Returns(playerDbSet);
            _dbContext.Setup(m => m.Death).Returns(dbSet);

            // act
            _deathService.Create(4, new PostDeathDto());

            // assert
            _dbContext.Verify(m => m.Location.Add(It.IsAny<Location>()), Times.Once);
        }

        [Fact]
        public void Create_ShouldCallSaveChangesMethod()
        {
            // arrange
            List<Death> deaths = _deathsToDtos.Keys.ToList();
            DbSet<Death> dbSet = GetDbSet(deaths.AsQueryable());
            DbSet<Player> playerDbSet = GetDbSet(_players.AsQueryable());
            DbSet<Location> locationDbSet = GetDbSet(_locations.AsQueryable());

            _dbContext.Setup(m => m.Location).Returns(locationDbSet);
            _dbContext.Setup(m => m.Player).Returns(playerDbSet);
            _dbContext.Setup(m => m.Death).Returns(dbSet);

            // act
            _deathService.Create(4, new PostDeathDto());

            // assert
            _dbContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        [Fact]
        public void Update_ShouldCallUpdateMethodOnce()
        {
            // arrange
            List<Death> deaths = _deathsToDtos.Keys.ToList();
            DbSet<Death> dbSet = GetDbSet(deaths.AsQueryable());
            DbSet<Location> locationDbSet = GetDbSet(_locations.AsQueryable());

            _dbContext.Setup(m => m.Location).Returns(locationDbSet);
            _dbContext.Setup(m => m.Death).Returns(dbSet);

            // act
            _deathService.Update(1, new PutDeathDto());

            // assert
            _dbContext.Verify(m => m.Location.Update(It.IsAny<Location>()), Times.Once);
        }

        [Fact]
        public void Update_ShouldCallSaveChangesMethodOnce()
        {
            // arrange
            List<Death> deaths = _deathsToDtos.Keys.ToList();
            DbSet<Death> dbSet = GetDbSet(deaths.AsQueryable());
            DbSet<Location> locationDbSet = GetDbSet(_locations.AsQueryable());

            _dbContext.Setup(m => m.Location).Returns(locationDbSet);
            _dbContext.Setup(m => m.Death).Returns(dbSet);

            // act
            _deathService.Update(1, new PutDeathDto());

            // assert
            _dbContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [Fact]
        public void DeleteById_ShouldCallRemoveMethodOnce()
        {
            // arrange
            List<Death> deaths = _deathsToDtos.Keys.ToList();
            DbSet<Death> dbSet = GetDbSet(deaths.AsQueryable());

            _dbContext.Setup(m => m.Death).Returns(dbSet);

            // act
            _deathService.DeleteById(1);

            // assert
            _dbContext.Verify(m => m.Death.Remove(It.IsAny<Death>()), Times.Once);
        }

        [Fact]
        public void DeleteById_ShouldCallSaveChangesMethodOnce()
        {
            // arrange
            List<Death> deaths = _deathsToDtos.Keys.ToList();
            DbSet<Death> dbSet = GetDbSet(deaths.AsQueryable());

            _dbContext.Setup(m => m.Death).Returns(dbSet);

            // act
            _deathService.DeleteById(1);

            // assert
            _dbContext.Verify(m => m.SaveChanges(), Times.Once);
        }
    }
}
