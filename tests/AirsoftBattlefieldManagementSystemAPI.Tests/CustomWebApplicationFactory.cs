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
                
                context.Battle.Add(new Battle
                {
                    BattleId = 1,
                    IsActive = false,
                    Name = "Kursk",
                    RoomId = 1,
                });
                context.Battle.Add(new Battle
                {
                    BattleId = 2,
                    IsActive = true,
                    Name = "Rhine",
                    RoomId = 2,
                });
                
                context.PlayerLocation.Add(new PlayerLocation
                {
                    PlayerLocationId = 1,
                    LocationId = 1,
                    PlayerId = 1,
                    RoomId = 1
                });
                context.Location.Add(new Location
                {
                    LocationId = 1,
                    Longitude = 21,
                    Latitude = 45,
                    Accuracy = 10,
                    Bearing = 45,
                    Time = DateTime.Now,
                });
                context.PlayerLocation.Add(new PlayerLocation
                {
                    PlayerLocationId = 2,
                    LocationId = 2,
                    PlayerId = 1,
                    RoomId = 1
                });
                context.Location.Add(new Location
                {
                    LocationId = 2,
                    Longitude = 22.67m,
                    Latitude = 46,
                    Accuracy = 5,
                    Bearing = 58,
                    Time = DateTime.Now.AddSeconds(5),
                });
                context.PlayerLocation.Add(new PlayerLocation
                {
                    PlayerLocationId = 3,
                    LocationId = 3,
                    PlayerId = 1,
                    RoomId = 1
                });
                context.Location.Add(new Location
                {
                    LocationId = 3,
                    Longitude = 18.22m,
                    Latitude = 46.4m,
                    Accuracy = 15,
                    Bearing = 58,
                    Time = DateTime.Now.AddSeconds(10),
                });

                context.PlayerLocation.Add(new PlayerLocation
                {
                    PlayerLocationId = 4,
                    LocationId = 4,
                    PlayerId = 2,
                    RoomId = 1
                });
                context.Location.Add(new Location
                {
                    LocationId = 4,
                    Longitude = 32.02m,
                    Latitude = 40.4m,
                    Accuracy = 7,
                    Bearing = 268,
                    Time = DateTime.Now.AddSeconds(11),
                });
                
                context.PlayerLocation.Add(new PlayerLocation
                {
                    PlayerLocationId = 5,
                    LocationId = 5,
                    PlayerId = 3,
                    RoomId = 2
                });
                context.Location.Add(new Location
                {
                    LocationId = 5,
                    Longitude = -32.02m,
                    Latitude = -40.4m,
                    Accuracy = 7,
                    Bearing = 268,
                    Time = DateTime.Now.AddSeconds(3),
                });
                context.PlayerLocation.Add(new PlayerLocation
                {
                    PlayerLocationId = 6,
                    LocationId = 6,
                    PlayerId = 3,
                    RoomId = 2
                });
                context.Location.Add(new Location
                {
                    LocationId = 6,
                    Longitude = -35.02m,
                    Latitude = -39.42m,
                    Accuracy = 1,
                    Bearing = 230,
                    Time = DateTime.Now.AddSeconds(6),
                });
                
                context.PlayerLocation.Add(new PlayerLocation
                {
                    PlayerLocationId = 7,
                    LocationId = 7,
                    PlayerId = 6,
                    RoomId = 2
                });
                context.Location.Add(new Location
                {
                    LocationId = 7,
                    Longitude = -2.02m,
                    Latitude = -4.42m,
                    Accuracy = 1000,
                    Bearing = 230,
                    Time = DateTime.Now.AddSeconds(4),
                });
                
                context.Kill.Add(new Kill
                {
                    KillId = 1,
                    LocationId = 1,
                    PlayerId = 1,
                    RoomId = 1
                });
                context.Kill.Add(new Kill
                {
                    KillId = 6,
                    LocationId = 3,
                    PlayerId = 1,
                    RoomId = 1
                });
                context.Kill.Add(new Kill
                {
                    KillId = 2,
                    LocationId = 8,
                    PlayerId = 1,
                    RoomId = 1
                });
                context.Location.Add(new Location
                {
                    LocationId = 8,
                    Longitude = 22.862341m,
                    Latitude = 44.4325m,
                    Accuracy = 15,
                    Bearing = 80,
                    Time = DateTime.Now.AddSeconds(2.7),
                });
                
                context.Kill.Add(new Kill
                {
                    KillId = 3,
                    LocationId = 9,
                    PlayerId = 2,
                    RoomId = 1
                });
                context.Location.Add(new Location
                {
                    LocationId = 9,
                    Longitude = 56.23455m,
                    Latitude = -86.4325m,
                    Accuracy = 1500,
                    Bearing = 203,
                    Time = DateTime.Now.AddSeconds(4.4),
                });
                
                context.Kill.Add(new Kill
                {
                    KillId = 4,
                    LocationId = 5,
                    PlayerId = 3,
                    RoomId = 2
                });
                context.Kill.Add(new Kill
                {
                    KillId = 5,
                    LocationId = 10,
                    PlayerId = 3,
                    RoomId = 2
                });
                context.Location.Add(new Location
                {
                    LocationId = 10,
                    Longitude = -2.2m,
                    Latitude = -0.2137m,
                    Accuracy = 1,
                    Bearing = 180,
                    Time = DateTime.Now.AddSeconds(4.4),
                });
                
                context.Kill.Add(new Kill
                {
                    KillId = 7,
                    LocationId = 11,
                    PlayerId = 4,
                    RoomId = 2
                });
                context.Location.Add(new Location
                {
                    LocationId = 11,
                    Longitude = -124.2213m,
                    Latitude = -34.2137m,
                    Accuracy = 1,
                    Bearing = 180,
                    Time = DateTime.Now.AddSeconds(4.4),
                });
                
                context.Death.Add(new Death
                {
                    DeathId = 1,
                    LocationId = 1,
                    PlayerId = 1,
                    RoomId = 1
                });
                context.Death.Add(new Death
                {
                    DeathId = 6,
                    LocationId = 3,
                    PlayerId = 1,
                    RoomId = 1
                });
                context.Death.Add(new Death
                {
                    DeathId = 2,
                    LocationId = 8,
                    PlayerId = 1,
                    RoomId = 1
                });
                context.Death.Add(new Death
                {
                    DeathId = 3,
                    LocationId = 9,
                    PlayerId = 2,
                    RoomId = 1
                });
                context.Death.Add(new Death
                {
                    DeathId = 4,
                    LocationId = 5,
                    PlayerId = 3,
                    RoomId = 2
                });
                context.Death.Add(new Death
                {
                    DeathId = 5,
                    LocationId = 10,
                    PlayerId = 3,
                    RoomId = 2
                });
                context.Death.Add(new Death
                {
                    DeathId = 7,
                    LocationId = 11,
                    PlayerId = 4,
                    RoomId = 2
                });
                
                context.SaveChanges();
            }
        });
    }
        
}