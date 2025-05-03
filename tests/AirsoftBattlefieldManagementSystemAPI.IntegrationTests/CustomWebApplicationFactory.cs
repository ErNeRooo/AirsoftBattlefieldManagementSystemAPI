using System.Runtime.CompilerServices;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AirsoftBattlefieldManagementSystemAPI.IntegrationTests;

public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    private string _testMethodCallerName;
    public CustomWebApplicationFactory([CallerMemberName] string? testMethodCallerName = null)
    {
        _testMethodCallerName = testMethodCallerName;
    }
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(IDbContextOptionsConfiguration<BattleManagementSystemDbContext>));

            services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
            services.AddMvc(options => options.Filters.Add(new FakeUserFilter()));
            
            string databaseUniqueName = Guid.NewGuid().ToString();
            services.AddDbContext<BattleManagementSystemDbContext>(options => options.UseInMemoryDatabase(databaseUniqueName));
            var serviceProvider = services.BuildServiceProvider(); 
            
            var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<BattleManagementSystemDbContext>();
            
            context.Database.EnsureCreated();

            bool isDbEmpty = context.Account.Count() == 0;
            
            if (isDbEmpty)
            {
                context.Account.Add(new Account
                {
                    AccountId = 2137,
                    Email = "seededEmail1@test.com",
                    PasswordHash = "dsadasdwefx"
                });
                context.Account.Add(new Account
                {
                    AccountId = 1,
                    Email = "seededEmail2@test.com",
                    PasswordHash = "fafarafa"
                });
                context.Account.Add(new Account
                {
                    AccountId = 2,
                    Email = "seededEmail3@test.com",
                    PasswordHash = "fafarafa"
                });
                context.Account.Add(new Account
                {
                    AccountId = 3,
                    Email = "seededEmail4@test.com",
                    PasswordHash = "fafarafa"
                });

                context.Player.Add(new Player
                {
                    PlayerId = 1,
                    Name = "Seeded Name",
                    RoomId = 1,
                    TeamId = 1,
                });
                context.Player.Add(new Player
                {
                    PlayerId = 2,
                    Name = "Seeded Name",
                    RoomId = 1,
                    TeamId = 2,
                });
                context.Player.Add(new Player
                {
                    PlayerId = 3,
                    Name = "Seeded Name",
                    RoomId = 2,
                    TeamId = 3
                });
                
                context.Room.Add(new Room
                {
                    RoomId = 1,
                    JoinCode = "123456",
                    MaxPlayers = 100,
                    AdminPlayerId = 1,
                    PasswordHash = "fafarafa"
                });
                context.Room.Add(new Room
                {
                    RoomId = 2,
                    JoinCode = "213700",
                    MaxPlayers = 2,
                    AdminPlayerId = 2,
                    PasswordHash = "fafarafa"
                });
                
                context.Team.Add(new Team
                {
                    Name = "Jednorozec",
                    TeamId = 1,
                    RoomId = 1,
                    OfficerPlayerId = 1
                });
                context.Team.Add(new Team
                {
                    Name = "Pogorzel",
                    TeamId = 2,
                    RoomId = 1,
                    OfficerPlayerId = 2
                });
                context.Team.Add(new Team
                {
                    Name = "Blue",
                    TeamId = 3,
                    RoomId = 2,
                    OfficerPlayerId = 3
                });
                
                context.SaveChanges();
            }
        });
    }
        
}