using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Account;
using AirsoftBattlefieldManagementSystemAPI.Models.Validators.Account;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;

namespace AirsoftBattlefieldManagementSystemAPI.Tests.ValidatorsTests.AccountTests;

public class LogInAccountDtoValidatorTests
{
    private BattleManagementSystemDbContext _dbContext;
    
    public LogInAccountDtoValidatorTests()
    {
        var builder = new DbContextOptionsBuilder<BattleManagementSystemDbContext>();
        builder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        _dbContext = new BattleManagementSystemDbContext(builder.Options);
        Seed();
    }

    public void Seed()
    {
        _dbContext.Account.Add(new Account
        {
            Email = "test@test.com",
            PasswordHash = "test",
        });
        _dbContext.Account.Add(new Account
        {
            Email = "e@e.e",
            PasswordHash = "test",
        });
        _dbContext.Account.Add(new Account
        {
            Email = "a@a.aa",
            PasswordHash = "test",
        });
        _dbContext.Account.Add(new Account
        {
            Email = "@test.com",
            PasswordHash = "test",
        });
        _dbContext.Account.Add(new Account
        {
            Email = "test@.com",
            PasswordHash = "test",
        });
        _dbContext.Account.Add(new Account
        {
            Email = "test@test.",
            PasswordHash = "test",
        });
        _dbContext.Account.Add(new Account
        {
            Email = "@.com",
            PasswordHash = "test",
        });
        _dbContext.Account.Add(new Account
        {
            Email = "@.",
            PasswordHash = "test",
        });
        _dbContext.Account.Add(new Account
        {
            Email = "@test.",
            PasswordHash = "test",
        });
        _dbContext.Account.Add(new Account
        {
            Email = "pozdrawiam",
            PasswordHash = "test",
        });
        _dbContext.SaveChanges();
    }

    [Theory]
    [InlineData("test@test.com", "str0ng p@ssworD")]
    [InlineData("a@a.aa", "str0ng p@ssworD")]
    public void Validate_ForValidModel_ReturnsSuccess(string email, string password)
    {
        var model = new LoginAccountDto
        {
            Email = email,
            Password = password
        };
        
        var validator = new LoginAccountDtoValidator(_dbContext);

        var result = validator.TestValidate(model);
        
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Theory]
    [InlineData("passwordd")]
    [InlineData("somepassword")]
    [InlineData("somep^ssword")]
    [InlineData("s3mep%ssword")]
    [InlineData("somEp!ssword")]
    [InlineData("s7mepassword")]
    [InlineData("s6mePassword")]
    [InlineData("s0mep{ssword")]
    [InlineData("somePassword")]
    [InlineData("someP\"ssword")]
    [InlineData("-P@ssw0rd")]
    [InlineData("")]
    public void Validate_ForInvalidPassword_ReturnsFailure(string password)
    {
        var model = new LoginAccountDto
        {
            Email = "test@test.com",
            Password = password
        };
        
        var validator = new LoginAccountDtoValidator(_dbContext);

        var result = validator.TestValidate(model);
        
        result.ShouldHaveAnyValidationError();
    }
    
    [Theory]
    [InlineData("thatEmail@doesnt.exist")]
    [InlineData("@test.com")]
    [InlineData("test@.com")]
    [InlineData("test@test.")]
    [InlineData("@.com")]
    [InlineData("@.")]
    [InlineData("@test.")]
    [InlineData("test@.")]
    [InlineData("pozdrawiam")]
    [InlineData("")]
    public void Validate_ForInvalidEmail_ReturnsFailure(string email)
    {
        var model = new LoginAccountDto
        {
            Email = email,
            Password = "str0ng p@ssworD"
        };
        
        var validator = new LoginAccountDtoValidator(_dbContext);

        var result = validator.TestValidate(model);
        
        result.ShouldHaveAnyValidationError();
    }
}