using CoreWCF.Configuration;
using CoreWCF.Description;
using ISI_TP2_10444_SmartHealth_Data;
using ISI_TP2_10444_SmartHealth_SoapRules.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServiceModelServices();
builder.Services.AddServiceModelMetadata();
builder.Services.AddSingleton<RulesService>();

builder.Services.AddDbContext<SmartHealthContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // This prevents the code from entering an infinite loop. (Paciente -> Alerta -> Paciente...)
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<SmartHealthContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();

app.UseServiceModel(serviceBuilder =>
{
    serviceBuilder.AddService<RulesService>();
    serviceBuilder.AddServiceEndpoint<RulesService, ISI_TP2_10444_SmartHealth_SoapRules.Contracts.IRulesService >(
        new CoreWCF.BasicHttpBinding(),
        "/RulesService.svc");

    var metadata = app.Services.GetRequiredService<ServiceMetadataBehavior>();
    metadata.HttpGetEnabled = true;
});

app.Run();
