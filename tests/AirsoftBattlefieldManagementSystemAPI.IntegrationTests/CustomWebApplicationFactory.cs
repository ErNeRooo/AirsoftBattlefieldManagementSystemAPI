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
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(IDbContextOptionsConfiguration<BattleManagementSystemDbContext>));

            services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
            services.AddMvc(options => options.Filters.Add(new FakeUserFilter()));
            
            services.AddDbContext<BattleManagementSystemDbContext>(options => options.UseInMemoryDatabase("InMemoryDatabase"));
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

                context.Player.Add(new Player
                {
                    PlayerId = 1,
                    Name = "Seeded Name",
                    IsDead = false
                });
                context.SaveChanges();
            }
        });
    }
        
}