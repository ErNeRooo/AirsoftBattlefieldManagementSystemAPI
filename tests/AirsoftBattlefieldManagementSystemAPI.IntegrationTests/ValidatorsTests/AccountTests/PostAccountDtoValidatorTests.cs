using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Account;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Models.Validators.Account;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;

namespace AirsoftBattlefieldManagementSystemAPI.IntegrationTests.ValidatorsTests.AccountTests;

public class PostAccountDtoValidatorTests
{
    private BattleManagementSystemDbContext _dbContext;
    
    public PostAccountDtoValidatorTests()
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
            Email = "a@a.aa",
            PasswordHash = "test",
        });
        _dbContext.SaveChanges();
    }

    [Theory]
    [InlineData("thatEmail@doesnt.exist", "VerY-Str0ngP@ssw0rd")]
    [InlineData("w@w.ww", "--P@ssw0rd")]
    [InlineData("some@other.email", "some Other passw0rd")]
    public void Validate_ForValidModel_ReturnsSuccess(string email, string password)
    {
        var model = new PostAccountDto
        {
            Email = email,
            Password = password
        };
        
        var validator = new PostAccountDtoValidator(_dbContext);

        var result = validator.TestValidate(model);
        
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Theory]
    [InlineData("passwordd")]
    [InlineData("somepassword")]
    [InlineData("somep@ssword")]
    [InlineData("s0mep ssword")]
    [InlineData("somEp}ssword")]
    [InlineData("s9mepassword")]
    [InlineData("s2mePassword")]
    [InlineData("s5mep.ssword")]
    [InlineData("somePassword")]
    [InlineData("someP\"ssword")]
    [InlineData("-P@ssw0rd")]
    [InlineData("")]
    public void Validate_ForInvalidPassword_ReturnsFailure(string password)
    {
        var model = new PostAccountDto
        {
            Email = "test2@test.com",
            Password = password
        };
        
        var validator = new PostAccountDtoValidator(_dbContext);

        var result = validator.TestValidate(model);
        
        result.ShouldHaveAnyValidationError();
    }
    
    [Theory]
    [InlineData("test@test.com")]
    [InlineData("a@a.aa")]
    [InlineData("e@e.e")]
    [InlineData("@test.com")]
    [InlineData("test@.com")]
    [InlineData("test@test.")]
    [InlineData("@.com")]
    [InlineData("@.")]
    [InlineData("@test.")]
    [InlineData("test@.")]
    [InlineData("pozdrawiam")]
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa@a.aa")]
    [InlineData("te,st@a.aa")]
    [InlineData("te@st@a.aa")]
    [InlineData("te..st@a.aa")]
    [InlineData("te st@a.aa")]
    [InlineData("test@te st.test")]
    [InlineData("test@te st.te st")]
    [InlineData("test@test.test.")]
    [InlineData("")]
    public void Validate_ForInvalidEmail_ReturnsFailure(string email)
    {
        var model = new PostAccountDto
        {
            Email = email,
            Password = "str0ng p@ssworD"
        };
        
        var validator = new PostAccountDtoValidator(_dbContext);

        var result = validator.TestValidate(model);
        
        result.ShouldHaveAnyValidationError();
    }
}