using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using Microsoft.EntityFrameworkCore;

namespace AirsoftBattlefieldManagementSystemAPI.Tests;

public class TestDbContextFactory
{
    public BattleManagementSystemDbContext Create()
    {
        var options = new DbContextOptionsBuilder<BattleManagementSystemDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        
        var context = new BattleManagementSystemDbContext(options);
        
        context.Database.EnsureCreated();
        
        return context;
    }
}