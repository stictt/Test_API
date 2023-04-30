using Microsoft.EntityFrameworkCore;
using Test_API.Models;
using Test_API.Services;
using System.Text.Json.Serialization;
using PrimaryAggregatorService.Infrastructure;
using Test_API.Infrastructure;

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
        builder.Services.AddScoped<OrderRepository>();
        builder.Services.AddScoped<OrderValidationService>();


        builder.Services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.AddConfiguration(builder.Configuration.GetSection("Logging"));
            loggingBuilder.AddConsole();
        });

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/error");
            app.UseStatusCodePagesWithReExecute("/error/{0}");
        }
        app.AddSettings();

        app.UseSwagger(options =>
        {
            options.SerializeAsV2 = true;
        });
        app.UseSwaggerUI();


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