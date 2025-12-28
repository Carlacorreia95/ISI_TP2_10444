using ISI_TP2_10444_SmartHealth_Data;
using ISI_TP2_10444_SmartHealth_Data.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Database connection (SQLite)
builder.Services.AddDbContext<SmartHealthContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Controllers + JSON options
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "SmartHealth IoT Gateway API",
        Version = "v1",
        Description = "API for receiving wearable signals and generating alerts."
    });
});

// Mock SOAP rules engine
builder.Services.AddScoped<ISoapRulesClient, SoapRulesClient>();

var app = builder.Build();

// Enable Swagger for UI + JSON static endpoint
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SmartHealth IoT Gateway API v1");
    c.RoutePrefix = string.Empty; // Makes Swagger load at root
});


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();