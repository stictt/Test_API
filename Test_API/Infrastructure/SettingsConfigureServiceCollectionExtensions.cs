using Test_API.Models;

namespace PrimaryAggregatorService.Infrastructure
{
    public static class ConfigureWebApplicationExtensions
    {
        public static void AddSettings(
        this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var temp = scope.ServiceProvider.GetRequiredService<MarketContext>() ;// configuration in constructor
        }
    }
}
