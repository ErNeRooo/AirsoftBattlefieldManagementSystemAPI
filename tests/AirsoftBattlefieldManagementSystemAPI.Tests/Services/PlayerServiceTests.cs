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
    public class PlayerServiceTests
    {
        private readonly PlayerService _playerService;
        private readonly Mock<IBattleManagementSystemDbContext> _dbContext;
        private readonly Mock<IMapper> _mapper;
        private readonly Dictionary<Player, PlayerDto> _playersToDtos;

        public PlayerServiceTests()
        {
            _mapper = new Mock<IMapper>();
            _dbContext = new Mock<IBattleManagementSystemDbContext>();
            _playerService = new PlayerService(_dbContext.Object, _mapper.Object);

            _mapper.Setup(m => m.Map<PlayerDto>(It.IsAny<Player>())).Returns(
                (Player p) => new PlayerDto
                {
                    PlayerId = p.PlayerId,
                    Name = p.Name,
                    IsDead = p.IsDead
                });
            _mapper.Setup(m => m.Map<Player>(It.IsAny<CreatePlayerDto>())).Returns(
                (CreatePlayerDto p) => new Player
                {
                    Name = p.Name,
                    IsDead = p.IsDead,
                });

            _playersToDtos = new Dictionary<Player, PlayerDto>()
            {
                {
                    new Player() { PlayerId = 1, Name = "ErNeRooo", IsDead = false }, 
                    new PlayerDto() { PlayerId = 1, Name = "ErNeRooo", IsDead = false }
                },
                {
                    new Player() { PlayerId = 2, Name = "Rayz", IsDead = true },
                    new PlayerDto() { PlayerId = 2, Name = "Rayz", IsDead = true }
                }
            };
        }

        private DbSet<Player> GetDbSet(IQueryable<Player> players)
        {
            var dbSetMock = new Mock<DbSet<Player>>();
            var playersMock = dbSetMock.As<IQueryable<Player>>();

            playersMock.Setup(m => m.Provider).Returns(players.Provider);
            playersMock.Setup(m => m.Expression).Returns(players.Expression);
            playersMock.Setup(m => m.ElementType).Returns(players.ElementType);
            playersMock.Setup(m => m.GetEnumerator()).Returns(players.GetEnumerator());

            return dbSetMock.Object;
        }

        [Theory]
        [InlineData(1, "ErNeRooo", false)]
        [InlineData(2, "Rayz", true)]
        public void GetById_ForExistingPlayerId_ReturnsPlayerDto(int id, string name, bool isDead)
        {
            // arrange
            IQueryable<Player> players = _playersToDtos.Keys.AsQueryable();
            DbSet<Player> dbSet = GetDbSet(players);
            _dbContext.Setup(m => m.Player).Returns(dbSet);

            // act
            PlayerDto result = _playerService.GetById(id);

            // assert
            result.ShouldSatisfyAllConditions(
                result => result.PlayerId.ShouldBe(id),
                result => result.Name.ShouldBe(name),
                result => result.IsDead.ShouldBe(isDead));
        }

        public static IEnumerable<object[]> Create_ForCreatePlayerDto_ReturnsIdOfCreatedPlayer_Data()
        {
            yield return new object[] { new CreatePlayerDto { Name = "Nick", IsDead = true, TeamId = 1 } };
            yield return new object[] { new CreatePlayerDto { Name = "Quasyn", IsDead = true, TeamId = 1 } };
        }

        [Theory]
        [MemberData(nameof(Create_ForCreatePlayerDto_ReturnsIdOfCreatedPlayer_Data))]
        public void Create_ForCreatePlayerDto_ReturnsIdOfCreatedPlayer(CreatePlayerDto createPlayerDto)
        {
            // arrange
            List<Player> players = _playersToDtos.Keys.ToList();
            DbSet<Player> dbSet = GetDbSet(players.AsQueryable());

            _dbContext.Setup(m => m.Player).Returns(dbSet);
            _dbContext.Setup(m => m.Player.Add(It.IsAny<Player>())).Callback(
                (Player player) =>
                {
                    player.PlayerId = dbSet.Count() + 1;
                    players.Add(player);
                });

            // act 
            int result = _playerService.Create(createPlayerDto);

            // assert
            result.ShouldBe(dbSet.Count());
        }

        [Fact]
        public void Create_ShouldCallAddMethodOnce()
        {
            // arrange
            List<Player> players = _playersToDtos.Keys.ToList();
            DbSet<Player> dbSet = GetDbSet(players.AsQueryable());

            _dbContext.Setup(m => m.Player).Returns(dbSet);

            // act
            _playerService.Create(new CreatePlayerDto());

            // assert
            _dbContext.Verify(m => m.Player.Add(It.IsAny<Player>()), Times.Once);
        }

        [Fact]
        public void Create_ShouldCallSaveChangesMethod()
        {
            // arrange
            List<Player> players = _playersToDtos.Keys.ToList();
            DbSet<Player> dbSet = GetDbSet(players.AsQueryable());

            _dbContext.Setup(m => m.Player).Returns(dbSet);

            // act
            _playerService.Create(new CreatePlayerDto());

            // assert
            _dbContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [Fact]
        public void Update_ShouldCallUpdateMethodOnce()
        {
            // arrange
            List<Player> players = _playersToDtos.Keys.ToList();
            DbSet<Player> dbSet = GetDbSet(players.AsQueryable());

            _dbContext.Setup(m => m.Player).Returns(dbSet);

            // act
            _playerService.Update(1, new UpdatePlayerDto());

            // assert
            _dbContext.Verify(m => m.Player.Update(It.IsAny<Player>()), Times.Once);
        }

        [Fact]
        public void Update_ShouldCallSaveChangesMethod()
        {
            // arrange
            List<Player> players = _playersToDtos.Keys.ToList();
            DbSet<Player> dbSet = GetDbSet(players.AsQueryable());

            _dbContext.Setup(m => m.Player).Returns(dbSet);

            // act
            _playerService.Update(1, new UpdatePlayerDto());

            // assert
            _dbContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [Fact]
        public void DeleteById_ShouldCallRemoveMethodOnce()
        {
            // arrange
            List<Player> players = _playersToDtos.Keys.ToList();
            DbSet<Player> dbSet = GetDbSet(players.AsQueryable());

            _dbContext.Setup(m => m.Player).Returns(dbSet);

            // act
            _playerService.DeleteById(1);

            // assert
            _dbContext.Verify(m => m.Player.Remove(It.IsAny<Player>()), Times.Once);
        }

        [Fact]
        public void DeleteById_ShouldCallRemoveChangesMethod()
        {
            // arrange
            List<Player> players = _playersToDtos.Keys.ToList();
            DbSet<Player> dbSet = GetDbSet(players.AsQueryable());

            _dbContext.Setup(m => m.Player).Returns(dbSet);

            // act
            _playerService.DeleteById(1);

            // assert
            _dbContext.Verify(m => m.SaveChanges(), Times.Once);
        }
    }
}
