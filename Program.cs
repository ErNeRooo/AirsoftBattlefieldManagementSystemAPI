using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Models.MappingProfiles;
using AirsoftBattlefieldManagementSystemAPI.Services;
using AutoMapper;

namespace AirsoftBattlefieldManagementSystemAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            builder.Services.AddDbContext<BmsDbContext>();
            builder.Services.AddSingleton<IMapper>(sp =>
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<PlayerMappingProfile>();
                });
                return config.CreateMapper();
            });
            builder.Services.AddScoped<IPlayerService, PlayerService>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
