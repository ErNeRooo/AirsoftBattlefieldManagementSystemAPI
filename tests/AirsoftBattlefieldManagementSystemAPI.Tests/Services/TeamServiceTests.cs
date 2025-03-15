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
    public class TeamServiceTests
    {
        private readonly TeamService _teamService;
        private readonly Mock<IBattleManagementSystemDbContext> _dbContext;
        private readonly Mock<IMapper> _mapper;
        private readonly Dictionary<Team, TeamDto> _teamsToDtos;

        public TeamServiceTests()
        {
            _mapper = new Mock<IMapper>();
            _dbContext = new Mock<IBattleManagementSystemDbContext>();
            _teamService = new TeamService(_mapper.Object, _dbContext.Object);

            _mapper.Setup(m => m.Map<TeamDto>(It.IsAny<Team>())).Returns(
                (Team t) => new TeamDto
                {
                    TeamId = t.TeamId,
                    Name = t.Name,
                    RoomId = t.RoomId
                });
            _mapper.Setup(m => m.Map<Team>(It.IsAny<CreateTeamDto>())).Returns(
                (CreateTeamDto t) => new Team
                {
                    Name = t.Name,
                    RoomId = t.RoomId
                });

            _teamsToDtos = new Dictionary<Team, TeamDto>()
            {
                {
                    new Team() { TeamId = 1, Name = "Jednorożeckie Siły Zbrojne", RoomId = 1}, 
                    new TeamDto() { TeamId = 1, Name = "Jednorożeckie Siły Zbrojne", RoomId = 1 }
                },
                {
                    new Team() { TeamId = 2, Name = "Pogorzelska Powstańcza Armia", RoomId = 2 },
                    new TeamDto() { TeamId = 2, Name = "Pogorzelska Powstańcza Armia", RoomId = 2 }
                }
            };
        }

        private DbSet<Team> GetDbSet(IQueryable<Team> teams)
        {
            var dbSetMock = new Mock<DbSet<Team>>();
            var teamsMock = dbSetMock.As<IQueryable<Team>>();

            teamsMock.Setup(m => m.Provider).Returns(teams.Provider);
            teamsMock.Setup(m => m.Expression).Returns(teams.Expression);
            teamsMock.Setup(m => m.ElementType).Returns(teams.ElementType);
            teamsMock.Setup(m => m.GetEnumerator()).Returns(teams.GetEnumerator());

            return dbSetMock.Object;
        }

        [Theory]
        [InlineData(1, "Jednorożeckie Siły Zbrojne", 1)]
        [InlineData(2, "Pogorzelska Powstańcza Armia", 2)]
        public void GetById_ForExistingTeamId_ReturnsTeamDto(int id, string name, int roomId)
        {
            // arrange
            IQueryable<Team> teams = _teamsToDtos.Keys.AsQueryable();
            DbSet<Team> dbSet = GetDbSet(teams);
            _dbContext.Setup(m => m.Team).Returns(dbSet);

            // act
            TeamDto result = _teamService.GetById(id);

            // assert
            result.ShouldSatisfyAllConditions(
                result => result.TeamId.ShouldBe(id),
                result => result.Name.ShouldBe(name),
                result => result.RoomId.ShouldBe(roomId));
        }

        public static IEnumerable<object[]> Create_ForCreateTeamDto_ReturnsIdOfCreatedTeam_Data()
        {
            yield return new object[] { new CreateTeamDto() };
            yield return new object[] { new CreateTeamDto() };
        }

        [Theory]
        [MemberData(nameof(Create_ForCreateTeamDto_ReturnsIdOfCreatedTeam_Data))]
        public void Create_ForCreateTeamDto_ReturnsIdOfCreatedTeam(CreateTeamDto createTeamDto)
        {
            // arrange
            List<Team> teams = _teamsToDtos.Keys.ToList();
            DbSet<Team> dbSet = GetDbSet(teams.AsQueryable());

            _dbContext.Setup(m => m.Team).Returns(dbSet);
            _dbContext.Setup(m => m.Team.Add(It.IsAny<Team>())).Callback(
                (Team team) =>
                {
                    team.TeamId = dbSet.Count() + 1;
                    teams.Add(team);
                });

            // act 
            int result = _teamService.Create(createTeamDto);

            // assert
            result.ShouldBe(dbSet.Count());
        }

        [Fact]
        public void Create_ShouldCallAddMethodOnce()
        {
            // arrange
            List<Team> teams = _teamsToDtos.Keys.ToList();
            DbSet<Team> dbSet = GetDbSet(teams.AsQueryable());

            _dbContext.Setup(m => m.Team).Returns(dbSet);

            // act
            _teamService.Create(new CreateTeamDto());

            // assert
            _dbContext.Verify(m => m.Team.Add(It.IsAny<Team>()), Times.Once);
        }

        [Fact]
        public void Create_ShouldCallSaveChangesMethod()
        {
            // arrange
            List<Team> teams = _teamsToDtos.Keys.ToList();
            DbSet<Team> dbSet = GetDbSet(teams.AsQueryable());

            _dbContext.Setup(m => m.Team).Returns(dbSet);

            // act
            _teamService.Create(new CreateTeamDto());

            // assert
            _dbContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [Fact]
        public void Update_ShouldCallUpdateMethodOnce()
        {
            // arrange
            List<Team> teams = _teamsToDtos.Keys.ToList();
            DbSet<Team> dbSet = GetDbSet(teams.AsQueryable());

            _dbContext.Setup(m => m.Team).Returns(dbSet);

            // act
            _teamService.Update(1, new UpdateTeamDto());

            // assert
            _dbContext.Verify(m => m.Team.Update(It.IsAny<Team>()), Times.Once);
        }

        [Fact]
        public void Update_ShouldCallSaveChangesMethod()
        {
            // arrange
            List<Team> teams = _teamsToDtos.Keys.ToList();
            DbSet<Team> dbSet = GetDbSet(teams.AsQueryable());

            _dbContext.Setup(m => m.Team).Returns(dbSet);

            // act
            _teamService.Update(1, new UpdateTeamDto());

            // assert
            _dbContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [Fact]
        public void DeleteById_ShouldCallRemoveMethodOnce()
        {
            // arrange
            List<Team> teams = _teamsToDtos.Keys.ToList();
            DbSet<Team> dbSet = GetDbSet(teams.AsQueryable());

            _dbContext.Setup(m => m.Team).Returns(dbSet);

            // act
            _teamService.DeleteById(1);

            // assert
            _dbContext.Verify(m => m.Team.Remove(It.IsAny<Team>()), Times.Once);
        }

        [Fact]
        public void DeleteById_ShouldCallRemoveChangesMethod()
        {
            // arrange
            List<Team> teams = _teamsToDtos.Keys.ToList();
            DbSet<Team> dbSet = GetDbSet(teams.AsQueryable());

            _dbContext.Setup(m => m.Team).Returns(dbSet);

            // act
            _teamService.DeleteById(1);

            // assert
            _dbContext.Verify(m => m.SaveChanges(), Times.Once);
        }
    }
}
