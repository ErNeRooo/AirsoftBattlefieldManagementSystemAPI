using System.Runtime.CompilerServices;
using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AirsoftBattlefieldManagementSystemAPI.Tests;

public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    public int PlayerId { get; set; }
    
    public CustomWebApplicationFactory(int playerId = 1)
    {
        PlayerId = playerId;
    }
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(IDbContextOptionsConfiguration<BattleManagementSystemDbContext>));

            services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
            services.AddMvc(options => options.Filters.Add(new FakeUserFilter(PlayerId)));
            
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
                    PasswordHash = "dsadasdwefx",
                    PlayerId = 2
                });
                context.Account.Add(new Account
                {
                    AccountId = 1,
                    Email = "seededEmail2@test.com",
                    PasswordHash = "fafarafa",
                    PlayerId = 4
                });
                context.Account.Add(new Account
                {
                    AccountId = 2,
                    Email = "seededEmail3@test.com",
                    PasswordHash = "fafarafa",
                });

                context.Player.Add(new Player
                {
                    PlayerId = 1,
                    Name = "Chisato",
                    IsDead = false,
                    RoomId = 1,
                    TeamId = 1,
                });
                context.Player.Add(new Player
                {
                    PlayerId = 2,
                    Name = "Takina",
                    IsDead = false,
                    RoomId = 1,
                    TeamId = 2,
                });
                context.Player.Add(new Player
                {
                    PlayerId = 3,
                    Name = "Ruby",
                    IsDead = true,
                    RoomId = 2,
                    TeamId = 3
                });
                context.Player.Add(new Player
                {
                    PlayerId = 4,
                    Name = "Aqua",
                    IsDead = true,
                    RoomId = 2,
                    TeamId = 3
                });
                context.Player.Add(new Player
                {
                    PlayerId = 5,
                    Name = "Nobody",
                    IsDead = true,
                    RoomId = 0,
                    TeamId = 0
                });
                context.Player.Add(new Player
                {
                    PlayerId = 6,
                    Name = "Akane",
                    IsDead = false,
                    RoomId = 2,
                    TeamId = 4
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
                    AdminPlayerId = 3,
                    PasswordHash = "fafarafa"
                });
                
                context.Team.Add(new Team
                {
                    Name = "Red",
                    TeamId = 1,
                    RoomId = 1,
                    OfficerPlayerId = 1
                });
                context.Team.Add(new Team
                {
                    Name = "Blue",
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
                context.Team.Add(new Team
                {
                    Name = "Navy",
                    TeamId = 4,
                    RoomId = 2,
                    OfficerPlayerId = 6
                });
                
                context.SaveChanges();
            }
        });
    }
        
}