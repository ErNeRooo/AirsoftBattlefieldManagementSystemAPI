using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirsoftBattlefieldManagementSystemAPI.Exceptions;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Services.Abstractions;
using AirsoftBattlefieldManagementSystemAPI.Services.Implementations;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Services
{
    public class RoomServiceTests
    {
        private readonly RoomService _roomService;
        private readonly Mock<IBattleManagementSystemDbContext> _dbContext;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IPasswordHasher<Room>> _passwordHasher;
        private readonly Mock<IJoinCodeService> _joinCodeService;
        private readonly Dictionary<Room, RoomDto> _roomsToDtos;

        public RoomServiceTests()
        {
            _mapper = new Mock<IMapper>();
            _dbContext = new Mock<IBattleManagementSystemDbContext>();
            _passwordHasher = new Mock<IPasswordHasher<Room>>();
            _joinCodeService = new Mock<IJoinCodeService>();
            _roomService = new RoomService(_mapper.Object, _dbContext.Object, _passwordHasher.Object, _joinCodeService.Object);

            _mapper.Setup(m => m.Map<RoomDto>(It.IsAny<Room>())).Returns(
                (Room r) => new RoomDto
                {
                    RoomId = r.RoomId,
                    MaxPlayers = r.MaxPlayers,
                    JoinCode = r.JoinCode
                });
            _mapper.Setup(m => m.Map<Room>(It.IsAny<CreateRoomDto>())).Returns(
                (CreateRoomDto r) => new Room
                {
                    MaxPlayers = r.MaxPlayers
                });

            _roomsToDtos = new Dictionary<Room, RoomDto>()
            {
                {
                    new Room() { RoomId = 1, MaxPlayers = 10, JoinCode = "021370"}, 
                    new RoomDto() { RoomId = 1, MaxPlayers = 10, JoinCode = "021370" }
                },
                {
                    new Room() { RoomId = 2, MaxPlayers = 1000, JoinCode = "000000" },
                    new RoomDto() { RoomId = 2, MaxPlayers = 1000, JoinCode = "000000" }
                }
            };
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

        [Theory]
        [InlineData(1, 10)]
        [InlineData(2, 1000)]
        public void GetById_ForExistingRoomId_ReturnsRoomDto(int id, int maxPlayers)
        {
            // arrange
            IQueryable<Room> rooms = _roomsToDtos.Keys.AsQueryable();
            DbSet<Room> dbSet = GetDbSet(rooms);
            _dbContext.Setup(m => m.Room).Returns(dbSet);

            // act
            RoomDto result = _roomService.GetById(id);

            // assert
            result.ShouldSatisfyAllConditions(
                result => result.RoomId.ShouldBe(id),
                result => result.MaxPlayers.ShouldBe(maxPlayers));
        }

        [Theory]
        [InlineData("021370", 10)]
        [InlineData("000000", 1000)]
        public void GetByJoinCode_ForExistingRoomId_ReturnsRoomDto(string joinCode, int maxPlayers)
        {
            // arrange
            IQueryable<Room> rooms = _roomsToDtos.Keys.AsQueryable();
            DbSet<Room> dbSet = GetDbSet(rooms);
            _dbContext.Setup(m => m.Room).Returns(dbSet);

            // act
            RoomDto result = _roomService.GetByJoinCode(joinCode);

            // assert
            result.ShouldSatisfyAllConditions(
                result => result.JoinCode.ShouldBe(joinCode),
                result => result.MaxPlayers.ShouldBe(maxPlayers));
        }

        [Theory]
        [InlineData("HELLNO")]
        [InlineData("000002")]
        public void GetByJoinCode_ForNotExistingRoomId_ThrowsNotFoundException(string joinNumber)
        {
            // arrange
            IQueryable<Room> rooms = _roomsToDtos.Keys.AsQueryable();
            DbSet<Room> dbSet = GetDbSet(rooms);
            _dbContext.Setup(m => m.Room).Returns(dbSet);

            // act & assert
            Should.Throw<NotFoundException>(() => _roomService.GetByJoinCode(joinNumber));
        }

        [Fact]
        public void Create_ForCreateRoomDto_ReturnsIdOfCreatedRoom()
        {
            // arrange
            List<Room> rooms = _roomsToDtos.Keys.ToList();
            DbSet<Room> roomDbSet = GetDbSet(rooms.AsQueryable());

            _dbContext.Setup(m => m.Room).Returns(roomDbSet);
            _dbContext.Setup(m => m.Room.Add(It.IsAny<Room>())).Callback(
                (Room room) =>
                {
                    room.RoomId = roomDbSet.Count() + 1;
                    rooms.Add(room);
                });

            // act 
            int result = _roomService.Create(new CreateRoomDto());

            // assert
            result.ShouldBe(roomDbSet.Count());
        }

        [Fact]
        public void Create_ShouldCallAddMethodOnce()
        {
            // arrange
            List<Room> rooms = _roomsToDtos.Keys.ToList();
            DbSet<Room> dbSet = GetDbSet(rooms.AsQueryable());

            _dbContext.Setup(m => m.Room).Returns(dbSet);

            // act
            _roomService.Create(new CreateRoomDto());

            // assert
            _dbContext.Verify(m => m.Room.Add(It.IsAny<Room>()), Times.Once);
        }

        [Fact]
        public void Create_ShouldCallSaveChangesMethod()
        {
            // arrange
            List<Room> rooms = _roomsToDtos.Keys.ToList();
            DbSet<Room> dbSet = GetDbSet(rooms.AsQueryable());

            _dbContext.Setup(m => m.Room).Returns(dbSet);

            // act
            _roomService.Create(new CreateRoomDto());

            // assert
            _dbContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [Fact]
        public void Update_ShouldCallUpdateMethodOnce()
        {
            // arrange
            List<Room> rooms = _roomsToDtos.Keys.ToList();
            DbSet<Room> dbSet = GetDbSet(rooms.AsQueryable());

            _dbContext.Setup(m => m.Room).Returns(dbSet);

            // act
            _roomService.Update(1, new UpdateRoomDto());

            // assert
            _dbContext.Verify(m => m.Room.Update(It.IsAny<Room>()), Times.Once);
        }

        [Fact]
        public void Update_ShouldCallSaveChangesMethodOnce()
        {
            // arrange
            List<Room> rooms = _roomsToDtos.Keys.ToList();
            DbSet<Room> dbSet = GetDbSet(rooms.AsQueryable());

            _dbContext.Setup(m => m.Room).Returns(dbSet);

            // act
            _roomService.Update(1, new UpdateRoomDto());

            // assert
            _dbContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [Fact]
        public void DeleteById_ShouldCallRemoveMethodOnce()
        {
            // arrange
            List<Room> rooms = _roomsToDtos.Keys.ToList();
            DbSet<Room> dbSet = GetDbSet(rooms.AsQueryable());

            _dbContext.Setup(m => m.Room).Returns(dbSet);

            // act
            _roomService.DeleteById(1);

            // assert
            _dbContext.Verify(m => m.Room.Remove(It.IsAny<Room>()), Times.Once);
        }

        [Fact]
        public void DeleteById_ShouldCallSaveChangesMethodOnce()
        {
            // arrange
            List<Room> rooms = _roomsToDtos.Keys.ToList();
            DbSet<Room> dbSet = GetDbSet(rooms.AsQueryable());

            _dbContext.Setup(m => m.Room).Returns(dbSet);

            // act
            _roomService.DeleteById(1);

            // assert
            _dbContext.Verify(m => m.SaveChanges(), Times.Once);
        }
    }
}
