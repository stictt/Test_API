using Microsoft.EntityFrameworkCore;
using System.Configuration;
using Test_API.Models;
using Test_API.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Net;
using Test_API.Models.Orders.Api;
using Microsoft.Extensions.Options;
using Microsoft.CodeAnalysis.Options;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Swashbuckle.AspNetCore.SwaggerUI;
using PrimaryAggregatorService.Infrastructure;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.EnableAnnotations();
        });

        builder.Services.AddControllers()
                .AddNewtonsoftJson()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

        builder.Services.AddDbContext<MarketContext>(
            options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddScoped<DatabaseService>();


        builder.Services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.AddConfiguration(builder.Configuration.GetSection("Logging"));
            loggingBuilder.AddConsole();
        });

        var app = builder.Build();
        app.AddSettings();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger(options =>
            {
                options.SerializeAsV2 = true;
            });
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
    public void ConfigureServices(IServiceCollection services)
    {
        var connString = WebApplication.CreateBuilder()
            .Configuration.GetConnectionString("DefaultConnection");


        object value = services.AddDbContext<MarketContext>(
            options => options.UseNpgsql(connString));
    }
}