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
    public class KillServiceTests
    {
        private readonly KillService _killService;
        private readonly Mock<IBattleManagementSystemDbContext> _dbContext;
        private readonly Mock<IMapper> _mapper;
        private readonly Dictionary<Kill, KillDto> _killsToDtos;
        private readonly List<Location> _locations;
        private readonly List<Player> _players;

        public KillServiceTests()
        {
            _mapper = new Mock<IMapper>();
            _dbContext = new Mock<IBattleManagementSystemDbContext>();
            _killService = new KillService(_mapper.Object, _dbContext.Object);

            _killsToDtos = new Dictionary<Kill, KillDto>()
            {
                {
                    new Kill() { KillId = 1, PlayerId = 4, LocationId = 2},
                    new KillDto() { KillId = 1, PlayerId = 4, LocationId = 2, Longitude = 64, Latitude = 21, Accuracy = 10, Bearing = 124, Time = new DateTime(1939, 9, 1, 5, 40, 0)}
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

            _mapper.Setup(m => m.Map<Location>(It.IsAny<CreateKillDto>())).Returns(
                (CreateKillDto k) => new Location
                {
                    Longitude = k.Longitude,
                    Latitude = k.Latitude,
                    Accuracy = k.Accuracy,
                    Bearing = k.Bearing,
                    Time = k.Time
                });

            _mapper.Setup(m => m.Map<UpdateKillDto, Location>(It.IsAny<UpdateKillDto>(), It.IsAny<Location>())).Returns(
                (UpdateKillDto src, Location dest) =>
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

            _mapper.Setup(m => m.Map<KillDto>(It.IsAny<Kill>())).Returns(
                (Kill k) => {
                    var location = _locations.FirstOrDefault(
                        l => l.LocationId == k.LocationId
                    );

                    return new KillDto
                    {
                        KillId = k.KillId,
                        LocationId = k.LocationId,
                        PlayerId = k.PlayerId,
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
        public static IEnumerable<object[]> GetKillTestData()
        {
            yield return new object[] { new KillDto() { KillId = 1, PlayerId = 4, LocationId = 2, Longitude = 64, Latitude = 21, Accuracy = 10, Bearing = 124, Time = new DateTime(1939, 9, 1, 5, 40, 0) } };
        }

        [Theory]
        [MemberData(nameof(GetKillTestData))]
        public void GetById_ForExistingKillId_ReturnsKillDto(KillDto expectedKill)
        {
            // arrange
            IQueryable<Kill> kills = _killsToDtos.Keys.AsQueryable();
            DbSet<Kill> killDbSet = GetDbSet(kills);
            DbSet<Location> locationDbSet = GetDbSet(_locations.AsQueryable());

            _dbContext.Setup(m => m.Location).Returns(locationDbSet);
            _dbContext.Setup(m => m.Kill).Returns(killDbSet);

            // act
            KillDto result = _killService.GetById(expectedKill.KillId);

            // assert
            result.ShouldSatisfyAllConditions(
                result => result.KillId.ShouldBe(expectedKill.KillId),
                result => result.PlayerId.ShouldBe(expectedKill.PlayerId),
                result => result.LocationId.ShouldBe(expectedKill.LocationId),
                result => result.Longitude.ShouldBe(expectedKill.Longitude),
                result => result.Latitude.ShouldBe(expectedKill.Latitude),
                result => result.Accuracy.ShouldBe(expectedKill.Accuracy),
                result => result.Bearing.ShouldBe(expectedKill.Bearing),
                result => result.Time.ShouldBe(expectedKill.Time));
        }

        [Theory]
        [InlineData(4)]
        public void GetAllOfPlayerWithId_ForExistingId_ReturnsNull(int id)
        {
            // arrange
            IQueryable<Kill> kills = _killsToDtos.Keys.AsQueryable();
            DbSet<Kill> killDbSet = GetDbSet(kills);
            DbSet<Location> locationDbSet = GetDbSet(_locations.AsQueryable());

            _dbContext.Setup(m => m.Location).Returns(locationDbSet);
            _dbContext.Setup(m => m.Kill).Returns(killDbSet);


            // act
            List<KillDto> result = _killService.GetAllOfPlayerWithId(id);

            // assert
            result.ShouldSatisfyAllConditions(
                result => result.Count.ShouldBe(1));
        }

        [Fact]
        public void Create_ForExistingPlayerId_ReturnsIdOfCreatedKill()
        {
            // arrange
            List<Kill> kills = _killsToDtos.Keys.ToList();
            DbSet<Kill> killDbSet = GetDbSet(kills.AsQueryable()); 
            DbSet<Player> playerDbSet = GetDbSet(_players.AsQueryable());
            DbSet<Location> locationDbSet = GetDbSet(_locations.AsQueryable());

            _dbContext.Setup(m => m.Location).Returns(locationDbSet);
            _dbContext.Setup(m => m.Player).Returns(playerDbSet);
            _dbContext.Setup(m => m.Kill).Returns(killDbSet);
            _dbContext.Setup(m => m.Kill.Add(It.IsAny<Kill>())).Callback(
                (Kill kill) =>
                {
                    kill.KillId = killDbSet.Count() + 1;
                    kills.Add(kill);
                });

            // act 
            int? result = _killService.Create(4, new CreateKillDto());

            // assert
            result.ShouldBe(killDbSet.Count());
        }

        [Fact]
        public void Create_ShouldCallAddMethodForKillOnce()
        {
            // arrange
            List<Kill> kills = _killsToDtos.Keys.ToList();
            DbSet<Kill> dbSet = GetDbSet(kills.AsQueryable());
            DbSet<Player> playerDbSet = GetDbSet(_players.AsQueryable());
            DbSet<Location> locationDbSet = GetDbSet(_locations.AsQueryable());

            _dbContext.Setup(m => m.Location).Returns(locationDbSet);
            _dbContext.Setup(m => m.Player).Returns(playerDbSet);
            _dbContext.Setup(m => m.Kill).Returns(dbSet);

            // act
            _killService.Create(4, new CreateKillDto());

            // assert
            _dbContext.Verify(m => m.Kill.Add(It.IsAny<Kill>()), Times.Once);
        }

        [Fact]
        public void Create_ShouldCallAddMethodForPlayerKillOnce()
        {
            // arrange
            List<Kill> kills = _killsToDtos.Keys.ToList();
            DbSet<Kill> dbSet = GetDbSet(kills.AsQueryable());
            DbSet<Player> playerDbSet = GetDbSet(_players.AsQueryable());
            DbSet<Location> locationDbSet = GetDbSet(_locations.AsQueryable());

            _dbContext.Setup(m => m.Location).Returns(locationDbSet);
            _dbContext.Setup(m => m.Player).Returns(playerDbSet);
            _dbContext.Setup(m => m.Kill).Returns(dbSet);

            // act
            _killService.Create(4, new CreateKillDto());

            // assert
            _dbContext.Verify(m => m.Location.Add(It.IsAny<Location>()), Times.Once);
        }

        [Fact]
        public void Create_ShouldCallSaveChangesMethod()
        {
            // arrange
            List<Kill> kills = _killsToDtos.Keys.ToList();
            DbSet<Kill> dbSet = GetDbSet(kills.AsQueryable());
            DbSet<Player> playerDbSet = GetDbSet(_players.AsQueryable());
            DbSet<Location> locationDbSet = GetDbSet(_locations.AsQueryable());

            _dbContext.Setup(m => m.Location).Returns(locationDbSet);
            _dbContext.Setup(m => m.Player).Returns(playerDbSet);
            _dbContext.Setup(m => m.Kill).Returns(dbSet);

            // act
            _killService.Create(4, new CreateKillDto());

            // assert
            _dbContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        public static IEnumerable<object[]> Update_ForNotExistingId_ReturnsFalse_Data()
        {
            yield return new object[] { 0, new UpdateKillDto {  } };
            yield return new object[] { 3, new UpdateKillDto {  } };
        }

        [Fact]
        public void Update_ShouldCallUpdateMethodOnce()
        {
            // arrange
            List<Kill> kills = _killsToDtos.Keys.ToList();
            DbSet<Kill> dbSet = GetDbSet(kills.AsQueryable());
            DbSet<Location> locationDbSet = GetDbSet(_locations.AsQueryable());

            _dbContext.Setup(m => m.Location).Returns(locationDbSet);
            _dbContext.Setup(m => m.Kill).Returns(dbSet);

            // act
            _killService.Update(1, new UpdateKillDto());

            // assert
            _dbContext.Verify(m => m.Location.Update(It.IsAny<Location>()), Times.Once);
        }

        [Fact]
        public void Update_ShouldCallSaveChangesMethodOnce()
        {
            // arrange
            List<Kill> kills = _killsToDtos.Keys.ToList();
            DbSet<Kill> dbSet = GetDbSet(kills.AsQueryable());
            DbSet<Location> locationDbSet = GetDbSet(_locations.AsQueryable());

            _dbContext.Setup(m => m.Location).Returns(locationDbSet);
            _dbContext.Setup(m => m.Kill).Returns(dbSet);

            // act
            _killService.Update(1, new UpdateKillDto());

            // assert
            _dbContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [Fact]
        public void DeleteById_ShouldCallRemoveMethodOnce()
        {
            // arrange
            List<Kill> kills = _killsToDtos.Keys.ToList();
            DbSet<Kill> dbSet = GetDbSet(kills.AsQueryable());

            _dbContext.Setup(m => m.Kill).Returns(dbSet);

            // act
            _killService.DeleteById(1);

            // assert
            _dbContext.Verify(m => m.Kill.Remove(It.IsAny<Kill>()), Times.Once);
        }

        [Fact]
        public void DeleteById_ShouldCallSaveChangesMethodOnce()
        {
            // arrange
            List<Kill> kills = _killsToDtos.Keys.ToList();
            DbSet<Kill> dbSet = GetDbSet(kills.AsQueryable());

            _dbContext.Setup(m => m.Kill).Returns(dbSet);

            // act
            _killService.DeleteById(1);

            // assert
            _dbContext.Verify(m => m.SaveChanges(), Times.Once);
        }
    }
}
