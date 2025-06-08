using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;

namespace AirsoftBattlefieldManagementSystemAPI.IntegrationTests;

public class ProgramTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly List<Type> _controllerTypes;

    public ProgramTests(WebApplicationFactory<Program> factory)
    {
        _controllerTypes = typeof(Program)
            .Assembly
            .GetTypes()
            .Where(type => type.IsSubclassOf(typeof(ControllerBase)))
            .ToList();
        
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                _controllerTypes.ForEach(c => services.AddScoped(c));
            });
        });
    }

    [Fact]
    public void ConfigureServices_ForControllers_RegistersAllControllers()
    {
        var scopedFactory = _factory.Services.GetService<IServiceScopeFactory>();
        using var scope = scopedFactory.CreateScope();
        
        _controllerTypes.ForEach(t =>
        {
            var controller = scope.ServiceProvider.GetService(t);
            controller.ShouldNotBeNull();
        });
    }
}