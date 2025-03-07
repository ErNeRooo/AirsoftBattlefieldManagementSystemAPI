using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Services.Implementations;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.Services;

public class AccountServiceTests
{
    private readonly Mock<IBattleManagementSystemDbContext> _dbContext;
    private readonly Mock<IMapper> _mapper;
    private readonly AccountService _accountService;

    public AccountServiceTests()
    {
        _dbContext = new Mock<IBattleManagementSystemDbContext>();

        _mapper = new Mock<IMapper>();
        _mapper.Setup(m => m.Map<AccountDto>(It.IsAny<Account>()))
            .Returns((Account src) => new AccountDto
            {
                AccountId = src.AccountId,
                Email = src.Email,
                Password = src.Password
            });

        _accountService = new AccountService(_mapper.Object, _dbContext.Object);
    }

    private Mock<DbSet<Account>> GetMockDbSet(IQueryable<Account> accounts)
    {
        var dbSet = new Mock<DbSet<Account>>();
        dbSet.As<IQueryable<Account>>().Setup(m => m.Provider).Returns(accounts.Provider);
        dbSet.As<IQueryable<Account>>().Setup(m => m.Expression).Returns(accounts.Expression);
        dbSet.As<IQueryable<Account>>().Setup(m => m.ElementType).Returns(accounts.ElementType);
        dbSet.As<IQueryable<Account>>().Setup(m => m.GetEnumerator()).Returns(accounts.GetEnumerator());

        return dbSet;
    }

    private Dictionary<Account, AccountDto> GetAccounts()
    {
        return new Dictionary<Account, AccountDto>
        {
            {
                new Account { AccountId = 1, Email = "some.email@gmail.com", Password = "inu123" },
                new AccountDto { AccountId = 1, Email = "some.email@gmail.com", Password = "inu123" }
            },
            {
                new Account { AccountId = 2, Email = "hellomum@gmail.com", Password = "neko321" },
                new AccountDto { AccountId = 2, Email = "hellomum@gmail.com", Password = "neko321" }
            }
        };
    }

    [Theory]
    [InlineData(1, "some.email@gmail.com", "inu123")]
    [InlineData(2, "hellomum@gmail.com", "neko321")]
    public void GetById_ForExistingId_ReturnsCorrectAccountDto(int id, string email, string password)
    {
        // arrange
        IQueryable<Account> accounts = GetAccounts().Keys.AsQueryable();
        Mock<DbSet<Account>> dbSet = GetMockDbSet(accounts);

        _dbContext.Setup(m => m.Account).Returns(dbSet.Object);

        // act
        var result = _accountService.GetById(id);

        // assert
        result.ShouldSatisfyAllConditions(
            () => result.AccountId.ShouldBe(id),
            () => result.Email.ShouldBe(email),
            () => result.Password.ShouldBe(password));
    }

    [Theory]
    [InlineData(78)]
    public void GetById_ForNonExistingId_ReturnsNull(int id)
    {
        // arrange
        IQueryable<Account> accounts = GetAccounts().Keys.AsQueryable();
        Mock<DbSet<Account>> dbSet = GetMockDbSet(accounts);

        _dbContext.Setup(m => m.Account).Returns(dbSet.Object);

        // act
        var result = _accountService.GetById(id);

        // assert
        result.ShouldBeNull();
    }
}
