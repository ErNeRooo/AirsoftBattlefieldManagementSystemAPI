using System;
using System.Collections.Generic;
using System.Linq;
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
    public class LocationServiceTests
    {
        private readonly LocationService _locationService;
        private readonly Mock<IBattleManagementSystemDbContext> _dbContext;
        private readonly Mock<IMapper> _mapper;
        private readonly Dictionary<Location, LocationDto> _locationsToDtos;
        private readonly List<PlayerLocation> _playerLocations;
        private readonly List<Player> _players;

        public LocationServiceTests()
        {
            _mapper = new Mock<IMapper>();
            _dbContext = new Mock<IBattleManagementSystemDbContext>();
            _locationService = new LocationService(_mapper.Object, _dbContext.Object);

            _locationsToDtos = new Dictionary<Location, LocationDto>()
            {
                {
                    new Location() { LocationId = 1, Longitude = 12, Latitude = 86, Accuracy = 15, Bearing = 180, Time = new DateTime(2025, 3, 11, 21, 37, 0)},
                    new LocationDto() { LocationId = 1,  Longitude = 12, Latitude = 86, Accuracy = 15, Bearing = 180, Time = new DateTime(2025, 3, 11, 21, 37, 0) }
                },
            };

            _playerLocations = new List<PlayerLocation>()
            {
                new PlayerLocation { LocationId = 1, PlayerId = 4 }
            };

            _players = new List<Player>()
            {
                new Player { PlayerId = 4}
            };

            _mapper.Setup(m => m.Map<Location>(It.IsAny<CreateLocationDto>())).Returns(
                (CreateLocationDto l) => new Location
                {
                    Longitude = l.Longitude,
                    Latitude = l.Latitude,
                    Accuracy = l.Accuracy,
                    Bearing = l.Bearing,
                    Time = l.Time
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
        public static IEnumerable<object[]> GetLocationTestData()
        {
            yield return new object[] { 1, 12, 86, (short)15, (short)180, new DateTime(2025, 3, 11, 21, 37, 0) };
        }

        [Theory]
        [MemberData(nameof(GetLocationTestData))]
        public void GetById_ForExistingLocationId_ReturnsLocationDto(int id, int longitude, int latitude, short accuracy, short bearing, DateTime time)
        {
            // arrange
            IQueryable<Location> locations = _locationsToDtos.Keys.AsQueryable();
            DbSet<Location> locationDbSet = GetDbSet(locations);
            DbSet<PlayerLocation> playerLocationDbSet = GetDbSet(_playerLocations.AsQueryable());

            _dbContext.Setup(m => m.PlayerLocation).Returns(playerLocationDbSet);
            _dbContext.Setup(m => m.Location).Returns(locationDbSet);


            // act
            LocationDto result = _locationService.GetById(id);

            // assert
            result.ShouldSatisfyAllConditions(
                result => result.LocationId.ShouldBe(id),
                result => result.Longitude.ShouldBe(longitude),
                result => result.Latitude.ShouldBe(latitude),
                result => result.Accuracy.ShouldBe(accuracy),
                result => result.Bearing.ShouldBe(bearing),
                result => result.Time.ShouldBe(time));
        }

        [Theory]
        [InlineData(78)]
        public void GetById_ForNonExistingId_ReturnsNull(int id)
        {
            // arrange
            IQueryable<Location> locations = _locationsToDtos.Keys.AsQueryable();
            DbSet<Location> locationDbSet = GetDbSet(locations);
            DbSet<PlayerLocation> playerLocationDbSet = GetDbSet(_playerLocations.AsQueryable());
            DbSet<Player> playerDbSet = GetDbSet(_players.AsQueryable());

            _dbContext.Setup(m => m.Player).Returns(playerDbSet);
            _dbContext.Setup(m => m.PlayerLocation).Returns(playerLocationDbSet);
            _dbContext.Setup(m => m.Location).Returns(locationDbSet);

            // act
            var result = _locationService.GetById(id);

            // assert
            result.ShouldBeNull();
        }

        [Theory]
        [InlineData(4)]
        public void GetAllOfPlayerWithId_ForExistingId_ReturnsNull(int id)
        {
            // arrange
            IQueryable<Location> locations = _locationsToDtos.Keys.AsQueryable();
            DbSet<Location> locationDbSet = GetDbSet(locations);
            DbSet<PlayerLocation> playerLocationDbSet = GetDbSet(_playerLocations.AsQueryable());

            _dbContext.Setup(m => m.PlayerLocation).Returns(playerLocationDbSet);
            _dbContext.Setup(m => m.Location).Returns(locationDbSet);


            // act
            List<LocationDto> result = _locationService.GetAllOfPlayerWithId(id);

            // assert
            result.ShouldSatisfyAllConditions(
                result => result.Count.ShouldBe(1));
        }

        [Theory]
        [InlineData(86)]
        public void GetAllOfPlayerWithId_ForNotExistingId_ReturnsNull(int id)
        {
            // arrange
            IQueryable<Location> locations = _locationsToDtos.Keys.AsQueryable();
            DbSet<Location> locationDbSet = GetDbSet(locations);
            DbSet<PlayerLocation> playerLocationDbSet = GetDbSet(_playerLocations.AsQueryable());

            _dbContext.Setup(m => m.PlayerLocation).Returns(playerLocationDbSet);
            _dbContext.Setup(m => m.Location).Returns(locationDbSet);


            // act
            List<LocationDto> result = _locationService.GetAllOfPlayerWithId(id);

            // assert
            result.ShouldBeNull();
        }

        [Fact]
        public void Create_ForExistingPlayerId_ReturnsIdOfCreatedLocation()
        {
            // arrange
            List<Location> locations = _locationsToDtos.Keys.ToList();
            DbSet<Location> locationDbSet = GetDbSet(locations.AsQueryable()); 
            DbSet<PlayerLocation> playerLocationDbSet = GetDbSet(_playerLocations.AsQueryable());
            DbSet<Player> playerDbSet = GetDbSet(_players.AsQueryable());

            _dbContext.Setup(m => m.Player).Returns(playerDbSet);
            _dbContext.Setup(m => m.PlayerLocation).Returns(playerLocationDbSet);
            _dbContext.Setup(m => m.Location).Returns(locationDbSet);
            _dbContext.Setup(m => m.Location.Add(It.IsAny<Location>())).Callback(
                (Location location) =>
                {
                    location.LocationId = locationDbSet.Count() + 1;
                    locations.Add(location);
                });

            // act 
            int? result = _locationService.Create(4, new CreateLocationDto());

            // assert
            result.ShouldBe(locationDbSet.Count());
        }

        [Fact]
        public void Create_ShouldCallAddMethodForLocationOnce()
        {
            // arrange
            List<Location> locations = _locationsToDtos.Keys.ToList();
            DbSet<Location> dbSet = GetDbSet(locations.AsQueryable());
            DbSet<PlayerLocation> playerLocationDbSet = GetDbSet(_playerLocations.AsQueryable());
            DbSet<Player> playerDbSet = GetDbSet(_players.AsQueryable());

            _dbContext.Setup(m => m.Player).Returns(playerDbSet);
            _dbContext.Setup(m => m.PlayerLocation).Returns(playerLocationDbSet);
            _dbContext.Setup(m => m.Location).Returns(dbSet);

            // act
            _locationService.Create(4, new CreateLocationDto());

            // assert
            _dbContext.Verify(m => m.Location.Add(It.IsAny<Location>()), Times.Once);
        }

        [Fact]
        public void Create_ShouldCallAddMethodForPlayerLocationOnce()
        {
            // arrange
            List<Location> locations = _locationsToDtos.Keys.ToList();
            DbSet<Location> dbSet = GetDbSet(locations.AsQueryable());
            DbSet<PlayerLocation> playerLocationDbSet = GetDbSet(_playerLocations.AsQueryable());
            DbSet<Player> playerDbSet = GetDbSet(_players.AsQueryable());

            _dbContext.Setup(m => m.Player).Returns(playerDbSet);
            _dbContext.Setup(m => m.PlayerLocation).Returns(playerLocationDbSet);
            _dbContext.Setup(m => m.Location).Returns(dbSet);

            // act
            _locationService.Create(4, new CreateLocationDto());

            // assert
            _dbContext.Verify(m => m.PlayerLocation.Add(It.IsAny<PlayerLocation>()), Times.Once);
        }

        [Fact]
        public void Create_ShouldCallSaveChangesMethod()
        {
            // arrange
            List<Location> locations = _locationsToDtos.Keys.ToList();
            DbSet<Location> dbSet = GetDbSet(locations.AsQueryable());
            DbSet<PlayerLocation> playerLocationDbSet = GetDbSet(_playerLocations.AsQueryable());
            DbSet<Player> playerDbSet = GetDbSet(_players.AsQueryable());

            _dbContext.Setup(m => m.Player).Returns(playerDbSet);
            _dbContext.Setup(m => m.PlayerLocation).Returns(playerLocationDbSet);
            _dbContext.Setup(m => m.Location).Returns(dbSet);

            // act
            _locationService.Create(4, new CreateLocationDto());

            // assert
            _dbContext.Verify(m => m.SaveChanges(), Times.AtLeast(2));
        }

        [Theory]
        [InlineData(1)]
        public void Update_ForExistingId_ReturnsTrue(int id)
        {
            // arrange
            List<Location> locations = _locationsToDtos.Keys.ToList();
            DbSet<Location> dbSet = GetDbSet(locations.AsQueryable());

            _dbContext.Setup(m => m.Location).Returns(dbSet);

            // act
            var result = _locationService.Update(id, new UpdateLocationDto());

            // assert
            result.ShouldBe(true);
        }

        public static IEnumerable<object[]> Update_ForNotExistingId_ReturnsFalse_Data()
        {
            yield return new object[] { 0, new UpdateLocationDto {  } };
            yield return new object[] { 3, new UpdateLocationDto {  } };
        }

        [Theory]
        [MemberData(nameof(Update_ForNotExistingId_ReturnsFalse_Data))]
        public void Update_ForNotExistingId_ReturnsFalse(int id, UpdateLocationDto locationDto)
        {
            // arrange
            List<Location> locations = _locationsToDtos.Keys.ToList();
            DbSet<Location> dbSet = GetDbSet(locations.AsQueryable());

            _dbContext.Setup(m => m.Location).Returns(dbSet);

            // act
            var result = _locationService.Update(id, locationDto);

            // assert
            result.ShouldBe(false);
        }

        [Fact]
        public void Update_ShouldCallUpdateMethodOnce()
        {
            // arrange
            List<Location> locations = _locationsToDtos.Keys.ToList();
            DbSet<Location> dbSet = GetDbSet(locations.AsQueryable());

            _dbContext.Setup(m => m.Location).Returns(dbSet);

            // act
            _locationService.Update(1, new UpdateLocationDto());

            // assert
            _dbContext.Verify(m => m.Location.Update(It.IsAny<Location>()), Times.Once);
        }

        [Fact]
        public void Update_ShouldCallSaveChangesMethodOnce()
        {
            // arrange
            List<Location> locations = _locationsToDtos.Keys.ToList();
            DbSet<Location> dbSet = GetDbSet(locations.AsQueryable());

            _dbContext.Setup(m => m.Location).Returns(dbSet);

            // act
            _locationService.Update(1, new UpdateLocationDto());

            // assert
            _dbContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [Theory]
        [InlineData(1)]
        public void DeleteById_ForExistingId_ReturnsTrue(int id)
        {
            // arrange
            List<Location> locations = _locationsToDtos.Keys.ToList();
            DbSet<Location> dbSet = GetDbSet(locations.AsQueryable());

            _dbContext.Setup(m => m.Location).Returns(dbSet);

            // act
            var result = _locationService.DeleteById(id);

            // assert
            result.ShouldBe(true);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(3)]
        public void DeleteById_ForNotExistingId_ReturnsFalse(int id)
        {
            // arrange
            List<Location> locations = _locationsToDtos.Keys.ToList();
            DbSet<Location> dbSet = GetDbSet(locations.AsQueryable());

            _dbContext.Setup(m => m.Location).Returns(dbSet);

            // act
            var result = _locationService.DeleteById(id);

            // assert
            result.ShouldBe(false);
        }

        [Fact]
        public void DeleteById_ShouldCallRemoveMethodOnce()
        {
            // arrange
            List<Location> locations = _locationsToDtos.Keys.ToList();
            DbSet<Location> dbSet = GetDbSet(locations.AsQueryable());

            _dbContext.Setup(m => m.Location).Returns(dbSet);

            // act
            _locationService.DeleteById(1);

            // assert
            _dbContext.Verify(m => m.Location.Remove(It.IsAny<Location>()), Times.Once);
        }

        [Fact]
        public void DeleteById_ShouldCallSaveChangesMethodOnce()
        {
            // arrange
            List<Location> locations = _locationsToDtos.Keys.ToList();
            DbSet<Location> dbSet = GetDbSet(locations.AsQueryable());

            _dbContext.Setup(m => m.Location).Returns(dbSet);

            // act
            _locationService.DeleteById(1);

            // assert
            _dbContext.Verify(m => m.SaveChanges(), Times.Once);
        }
    }
}
