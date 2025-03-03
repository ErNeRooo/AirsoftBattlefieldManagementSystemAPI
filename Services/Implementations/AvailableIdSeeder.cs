using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace AirsoftBattlefieldManagementSystemAPI.Services.Implementations
{
    public class AvailableIdSeeder(IBattleManagementSystemDbContext dbContext)
    {
        public void Seed()
        {
            if (dbContext.Database.CanConnect()) throw new Exception("Can't make it, THE DOOR STUCK!!!");
            
            if (dbContext.AvailableId.Any()) return;
                
            List<AvailableId> availableIds = new List<AvailableId>();

            int i = 0;
            while (i < 1000000)
            {
                var id = new AvailableId();
                id.Id = i;

                availableIds.Add(id);
                i++;
            }
            
            dbContext.AvailableId.AddRange(availableIds);
            dbContext.SaveChanges();
        }
    }
}
