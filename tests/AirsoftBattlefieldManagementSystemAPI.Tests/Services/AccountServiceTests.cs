using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Get;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update;
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
    private readonly Dictionary<Account, AccountDto> _accountsToDtos;

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
        _mapper.Setup(m => m.Map<Account>(It.IsAny<CreateAccountDto>()))
            .Returns((CreateAccountDto src) => new Account
            {
                AccountId = 0,
                Email = src.Email,
                Password = src.Password
            });
        _mapper.Setup(m => m.Map(It.IsAny<UpdateAccountDto>(), It.IsAny<Account>()))
            .Returns((UpdateAccountDto src, Account dest) => new Account
            {
                AccountId = dest.AccountId,
                Email = src.Email,
                Password = src.Password
            });

        _accountService = new AccountService(_mapper.Object, _dbContext.Object);

        _accountsToDtos = new Dictionary<Account, AccountDto>
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

    private DbSet<Account> GetDbSet(IQueryable<Account> accounts)
    {
        var dbSetMock = new Mock<DbSet<Account>>();
        var accountsMock = dbSetMock.As<IQueryable<Account>>();

        accountsMock.Setup(m => m.Provider).Returns(accounts.Provider);
        accountsMock.Setup(m => m.Expression).Returns(accounts.Expression);
        accountsMock.Setup(m => m.ElementType).Returns(accounts.ElementType);
        accountsMock.Setup(m => m.GetEnumerator()).Returns(accounts.GetEnumerator());

        return dbSetMock.Object;
    }

    [Theory]
    [InlineData(1, "some.email@gmail.com", "inu123")]
    [InlineData(2, "hellomum@gmail.com", "neko321")]
    public void GetById_ForExistingId_ReturnsCorrectAccountDto(int id, string email, string password)
    {
        // arrange
        IQueryable<Account> accounts = _accountsToDtos.Keys.AsQueryable();
        DbSet<Account> dbSet = GetDbSet(accounts);

        _dbContext.Setup(m => m.Account).Returns(dbSet);

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
        IQueryable<Account> accounts = _accountsToDtos.Keys.AsQueryable();
        DbSet<Account> dbSet = GetDbSet(accounts);

        _dbContext.Setup(m => m.Account).Returns(dbSet);

        // act
        var result = _accountService.GetById(id);

        // assert
        result.ShouldBeNull();
    }

    public static IEnumerable<object[]> Create_ForCreateAccountDto_ReturnsIdOfCreatedAccount_Data()
    {
        yield return new object[]
        {
            new CreateAccountDto { Email = "haha@example.com", Password = "tori098" }
        };
        yield return new object[]
        {
            new CreateAccountDto { Email = "", Password = "" }
        };
    }

    [Theory]
    [MemberData(nameof(Create_ForCreateAccountDto_ReturnsIdOfCreatedAccount_Data))]
    public void Create_ForCreateAccountDto_ReturnsIdOfCreatedAccount(CreateAccountDto accountDto)
    {
        // arrange
        List<Account> accounts = _accountsToDtos.Keys.ToList();
        DbSet<Account> dbSet = GetDbSet(accounts.AsQueryable());

        _dbContext.Setup(m => m.Account).Returns(dbSet);
        _dbContext.Setup(m => m.Account.Add(It.IsAny<Account>())).Callback(
            (Account account) =>
            {
                int id = accounts.Count + 1;

                account.AccountId = id;
                accounts.Add(account);
            });

        // act
        var result = _accountService.Create(accountDto);

        // assert
        result.ShouldBe(accounts.Count);
    }

    [Fact]
    public void Create_ShouldCallAddMethodOnce()
    {
        // arrange
        List<Account> accounts = _accountsToDtos.Keys.ToList();
        DbSet<Account> dbSet = GetDbSet(accounts.AsQueryable());

        _dbContext.Setup(m => m.Account).Returns(dbSet);

        // act
        _accountService.Create(new CreateAccountDto());

        // assert
        _dbContext.Verify(m => m.Account.Add(It.IsAny<Account>()), Times.Once);
    }

    [Fact]
    public void Create_ShouldCallSaveChangesMethod()
    {
        // arrange
        List<Account> accounts = _accountsToDtos.Keys.ToList();
        DbSet<Account> dbSet = GetDbSet(accounts.AsQueryable());

        _dbContext.Setup(m => m.Account).Returns(dbSet);

        // act
        _accountService.Create(new CreateAccountDto());

        // assert
        _dbContext.Verify(m => m.SaveChanges(), Times.Once);
    }


    public static IEnumerable<object[]> Update_ForExistingId_ReturnsTrue_Data()
    {
        yield return new object[]
        {
            1,
            new UpdateAccountDto { Email = "haha@example.com", Password = "tori098" }
        };
        yield return new object[]
        {
            2,
            new UpdateAccountDto { Email = "", Password = "" }
        };
    }

    [Theory]
    [MemberData(nameof(Update_ForExistingId_ReturnsTrue_Data))]
    public void Update_ForExistingId_ReturnsTrue(int id, UpdateAccountDto accountDto)
    {
        // arrange
        List<Account> accounts = _accountsToDtos.Keys.ToList();
        DbSet<Account> dbSet = GetDbSet(accounts.AsQueryable());

        _dbContext.Setup(m => m.Account).Returns(dbSet);

        // act
        var result = _accountService.Update(id, accountDto);

        // assert
        result.ShouldBe(true);
    }

    public static IEnumerable<object[]> Update_ForNotExistingId_ReturnsFalse_Data()
    {
        yield return new object[]
        {
            0,
            new UpdateAccountDto { Email = "haha@example.com", Password = "tori098" }
        };
        yield return new object[]
        {
            3,
            new UpdateAccountDto { Email = "", Password = "" }
        };
    }

    [Theory]
    [MemberData(nameof(Update_ForNotExistingId_ReturnsFalse_Data))]
    public void Update_ForNotExistingId_ReturnsFalse(int id, UpdateAccountDto accountDto)
    {
        // arrange
        List<Account> accounts = _accountsToDtos.Keys.ToList();
        DbSet<Account> dbSet = GetDbSet(accounts.AsQueryable());

        _dbContext.Setup(m => m.Account).Returns(dbSet);

        // act
        var result = _accountService.Update(id, accountDto);

        // assert
        result.ShouldBe(false);
    }

    [Fact]
    public void Update_ShouldCallUpdateMethodOnce()
    {
        // arrange
        List<Account> accounts = _accountsToDtos.Keys.ToList();
        DbSet<Account> dbSet = GetDbSet(accounts.AsQueryable());

        _dbContext.Setup(m => m.Account).Returns(dbSet);

        // act
        _accountService.Update(1, new UpdateAccountDto());

        // assert
        _dbContext.Verify(m => m.Account.Update(It.IsAny<Account>()), Times.Once);
    }

    [Fact]
    public void Update_ShouldCallSaveChangesMethod()
    {
        // arrange
        List<Account> accounts = _accountsToDtos.Keys.ToList();
        DbSet<Account> dbSet = GetDbSet(accounts.AsQueryable());

        _dbContext.Setup(m => m.Account).Returns(dbSet);

        // act
        _accountService.Update(1, new UpdateAccountDto());

        // assert
        _dbContext.Verify(m => m.SaveChanges(), Times.Once);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public void DeleteById_ForExistingId_ReturnsTrue(int id)
    {
        // arrange
        List<Account> accounts = _accountsToDtos.Keys.ToList();
        DbSet<Account> dbSet = GetDbSet(accounts.AsQueryable());

        _dbContext.Setup(m => m.Account).Returns(dbSet);

        // act
        var result = _accountService.DeleteById(id);

        // assert
        result.ShouldBe(true);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(3)]
    public void DeleteById_ForNotExistingId_ReturnsFalse(int id)
    {
        // arrange
        List<Account> accounts = _accountsToDtos.Keys.ToList();
        DbSet<Account> dbSet = GetDbSet(accounts.AsQueryable());

        _dbContext.Setup(m => m.Account).Returns(dbSet);

        // act
        var result = _accountService.DeleteById(id);

        // assert
        result.ShouldBe(false);
    }

    [Fact]
    public void DeleteById_ShouldCallRemoveMethodOnce()
    {
        // arrange
        List<Account> accounts = _accountsToDtos.Keys.ToList();
        DbSet<Account> dbSet = GetDbSet(accounts.AsQueryable());

        _dbContext.Setup(m => m.Account).Returns(dbSet);

        // act
        _accountService.DeleteById(1);

        // assert
        _dbContext.Verify(m => m.Account.Remove(It.IsAny<Account>()), Times.Once);
    }

    [Fact]
    public void DeleteById_ShouldCallRemoveChangesMethod()
    {
        // arrange
        List<Account> accounts = _accountsToDtos.Keys.ToList();
        DbSet<Account> dbSet = GetDbSet(accounts.AsQueryable());

        _dbContext.Setup(m => m.Account).Returns(dbSet);

        // act
        _accountService.DeleteById(1);

        // assert
        _dbContext.Verify(m => m.SaveChanges(), Times.Once);
    }

}
