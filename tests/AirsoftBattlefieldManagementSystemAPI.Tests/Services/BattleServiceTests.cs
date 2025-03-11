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
    public class BattleServiceTests
    {
        private readonly BattleService _battleService;
        private readonly Mock<IBattleManagementSystemDbContext> _dbContext;
        private readonly Mock<IMapper> _mapper;
        private readonly Dictionary<Battle, BattleDto> _battlesToDtos;

        public BattleServiceTests()
        {
            _mapper = new Mock<IMapper>();
            _dbContext = new Mock<IBattleManagementSystemDbContext>();
            _battleService = new BattleService(_mapper.Object, _dbContext.Object);

            _mapper.Setup(m => m.Map<BattleDto>(It.IsAny<Battle>())).Returns(
                (Battle b) => new BattleDto
                {
                    BattleId = b.BattleId,
                    Name = b.Name,
                    IsActive = b.IsActive,
                    RoomId = b.RoomId
                });
            _mapper.Setup(m => m.Map<Battle>(It.IsAny<CreateBattleDto>())).Returns(
                (CreateBattleDto b) => new Battle
                {
                    Name = b.Name,
                    IsActive = b.IsActive,
                    RoomId = b.RoomId
                });

            _battlesToDtos = new Dictionary<Battle, BattleDto>()
            {
                {
                    new Battle() { BattleId = 1, Name = "Bitwa na łuku Słabogórskim", IsActive = false, RoomId = 1 }, 
                    new BattleDto() { BattleId = 1, Name = "Bitwa na łuku Słabogórskim", IsActive = false, RoomId = 1 }
                },
                {
                    new Battle() { BattleId = 2, Name = "Bitwa pod pogorzelą", IsActive = true, RoomId = 2 },
                    new BattleDto() { BattleId = 2, Name = "Bitwa pod pogorzelą", IsActive = true, RoomId = 2 }
                }
            };
        }

        private DbSet<Battle> GetDbSet(IQueryable<Battle> battles)
        {
            var dbSetMock = new Mock<DbSet<Battle>>();
            var battlesMock = dbSetMock.As<IQueryable<Battle>>();

            battlesMock.Setup(m => m.Provider).Returns(battles.Provider);
            battlesMock.Setup(m => m.Expression).Returns(battles.Expression);
            battlesMock.Setup(m => m.ElementType).Returns(battles.ElementType);
            battlesMock.Setup(m => m.GetEnumerator()).Returns(battles.GetEnumerator());

            return dbSetMock.Object;
        }

        [Theory]
        [InlineData(1, "Bitwa na łuku Słabogórskim", false, 1)]
        [InlineData(2, "Bitwa pod pogorzelą", true, 2)]
        public void GetById_ForExistingBattleId_ReturnsBattleDto(int id, string name, bool isActive, int roomId)
        {
            // arrange
            IQueryable<Battle> battles = _battlesToDtos.Keys.AsQueryable();
            DbSet<Battle> dbSet = GetDbSet(battles);
            _dbContext.Setup(m => m.Battle).Returns(dbSet);

            // act
            BattleDto result = _battleService.GetById(id);

            // assert
            result.ShouldSatisfyAllConditions(
                result => result.BattleId.ShouldBe(id),
                result => result.Name.ShouldBe(name),
                result => result.IsActive.ShouldBe(isActive),
                result => result.RoomId.ShouldBe(roomId));
        }

        [Theory]
        [InlineData(78)]
        public void GetById_ForNonExistingId_ReturnsNull(int id)
        {
            // arrange
            IQueryable<Battle> battles = _battlesToDtos.Keys.AsQueryable();
            DbSet<Battle> dbSet = GetDbSet(battles);

            _dbContext.Setup(m => m.Battle).Returns(dbSet);

            // act
            var result = _battleService.GetById(id);

            // assert
            result.ShouldBeNull();
        }

        public static IEnumerable<object[]> Create_ForCreateBattleDto_ReturnsIdOfCreatedBattle_Data()
        {
            yield return new object[] { new CreateBattleDto { Name = "Bitwa Małowidzka", IsActive = true, RoomId = 4 } };
            yield return new object[] { new CreateBattleDto { Name = "Ofensywa Lipska", IsActive = false, RoomId = 1 } };
        }

        [Theory]
        [MemberData(nameof(Create_ForCreateBattleDto_ReturnsIdOfCreatedBattle_Data))]
        public void Create_ForCreateBattleDto_ReturnsIdOfCreatedBattle(CreateBattleDto createBattleDto)
        {
            // arrange
            List<Battle> battles = _battlesToDtos.Keys.ToList();
            DbSet<Battle> dbSet = GetDbSet(battles.AsQueryable());

            _dbContext.Setup(m => m.Battle).Returns(dbSet);
            _dbContext.Setup(m => m.Battle.Add(It.IsAny<Battle>())).Callback(
                (Battle battle) =>
                {
                    battle.BattleId = dbSet.Count() + 1;
                    battles.Add(battle);
                });

            // act 
            int result = _battleService.Create(createBattleDto);

            // assert
            result.ShouldBe(dbSet.Count());
        }

        [Fact]
        public void Create_ShouldCallAddMethodOnce()
        {
            // arrange
            List<Battle> battles = _battlesToDtos.Keys.ToList();
            DbSet<Battle> dbSet = GetDbSet(battles.AsQueryable());

            _dbContext.Setup(m => m.Battle).Returns(dbSet);

            // act
            _battleService.Create(new CreateBattleDto());

            // assert
            _dbContext.Verify(m => m.Battle.Add(It.IsAny<Battle>()), Times.Once);
        }

        [Fact]
        public void Create_ShouldCallSaveChangesMethod()
        {
            // arrange
            List<Battle> battles = _battlesToDtos.Keys.ToList();
            DbSet<Battle> dbSet = GetDbSet(battles.AsQueryable());

            _dbContext.Setup(m => m.Battle).Returns(dbSet);

            // act
            _battleService.Create(new CreateBattleDto());

            // assert
            _dbContext.Verify(m => m.SaveChanges(), Times.Once);
        }


        public static IEnumerable<object[]> Update_ForExistingId_ReturnsTrue_Data()
        {
            yield return new object[] { 1, new UpdateBattleDto { } };
            yield return new object[] { 2, new UpdateBattleDto { } };
        }

        [Theory]
        [MemberData(nameof(Update_ForExistingId_ReturnsTrue_Data))]
        public void Update_ForExistingId_ReturnsTrue(int id, UpdateBattleDto battleDto)
        {
            // arrange
            List<Battle> battles = _battlesToDtos.Keys.ToList();
            DbSet<Battle> dbSet = GetDbSet(battles.AsQueryable());

            _dbContext.Setup(m => m.Battle).Returns(dbSet);

            // act
            var result = _battleService.Update(id, battleDto);

            // assert
            result.ShouldBe(true);
        }

        public static IEnumerable<object[]> Update_ForNotExistingId_ReturnsFalse_Data()
        {
            yield return new object[] { 0, new UpdateBattleDto {  } };
            yield return new object[] { 3, new UpdateBattleDto {  } };
        }

        [Theory]
        [MemberData(nameof(Update_ForNotExistingId_ReturnsFalse_Data))]
        public void Update_ForNotExistingId_ReturnsFalse(int id, UpdateBattleDto battleDto)
        {
            // arrange
            List<Battle> battles = _battlesToDtos.Keys.ToList();
            DbSet<Battle> dbSet = GetDbSet(battles.AsQueryable());

            _dbContext.Setup(m => m.Battle).Returns(dbSet);

            // act
            var result = _battleService.Update(id, battleDto);

            // assert
            result.ShouldBe(false);
        }

        [Fact]
        public void Update_ShouldCallUpdateMethodOnce()
        {
            // arrange
            List<Battle> battles = _battlesToDtos.Keys.ToList();
            DbSet<Battle> dbSet = GetDbSet(battles.AsQueryable());

            _dbContext.Setup(m => m.Battle).Returns(dbSet);

            // act
            _battleService.Update(1, new UpdateBattleDto());

            // assert
            _dbContext.Verify(m => m.Battle.Update(It.IsAny<Battle>()), Times.Once);
        }

        [Fact]
        public void Update_ShouldCallSaveChangesMethod()
        {
            // arrange
            List<Battle> battles = _battlesToDtos.Keys.ToList();
            DbSet<Battle> dbSet = GetDbSet(battles.AsQueryable());

            _dbContext.Setup(m => m.Battle).Returns(dbSet);

            // act
            _battleService.Update(1, new UpdateBattleDto());

            // assert
            _dbContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void DeleteById_ForExistingId_ReturnsTrue(int id)
        {
            // arrange
            List<Battle> battles = _battlesToDtos.Keys.ToList();
            DbSet<Battle> dbSet = GetDbSet(battles.AsQueryable());

            _dbContext.Setup(m => m.Battle).Returns(dbSet);

            // act
            var result = _battleService.DeleteById(id);

            // assert
            result.ShouldBe(true);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(3)]
        public void DeleteById_ForNotExistingId_ReturnsFalse(int id)
        {
            // arrange
            List<Battle> battles = _battlesToDtos.Keys.ToList();
            DbSet<Battle> dbSet = GetDbSet(battles.AsQueryable());

            _dbContext.Setup(m => m.Battle).Returns(dbSet);

            // act
            var result = _battleService.DeleteById(id);

            // assert
            result.ShouldBe(false);
        }

        [Fact]
        public void DeleteById_ShouldCallRemoveMethodOnce()
        {
            // arrange
            List<Battle> battles = _battlesToDtos.Keys.ToList();
            DbSet<Battle> dbSet = GetDbSet(battles.AsQueryable());

            _dbContext.Setup(m => m.Battle).Returns(dbSet);

            // act
            _battleService.DeleteById(1);

            // assert
            _dbContext.Verify(m => m.Battle.Remove(It.IsAny<Battle>()), Times.Once);
        }

        [Fact]
        public void DeleteById_ShouldCallRemoveChangesMethod()
        {
            // arrange
            List<Battle> battles = _battlesToDtos.Keys.ToList();
            DbSet<Battle> dbSet = GetDbSet(battles.AsQueryable());

            _dbContext.Setup(m => m.Battle).Returns(dbSet);

            // act
            _battleService.DeleteById(1);

            // assert
            _dbContext.Verify(m => m.SaveChanges(), Times.Once);
        }
    }
}
