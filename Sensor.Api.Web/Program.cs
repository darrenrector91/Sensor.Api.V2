using Sensor.Api.Data;
using Sensor.Api.Web.Services;
using Sensor.Api.Web.Services.Interfaces;

using Sensor.Api.Data.Repositories;
using Sensor.Api.Data.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

const string AngularDevCorsPolicy = "AngularDevCorsPolicy";

builder.Services.AddCors(options =>
{
    options.AddPolicy(AngularDevCorsPolicy, policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:4200",
                "http://192.168.5.103")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ISensorDbContext, SensorDbContext>();

builder.Services.AddScoped<IControllerRepository, ControllerRepository>();
builder.Services.AddScoped<ISensorRepository, SensorRepository>();
builder.Services.AddScoped<ISensorMeasurementRepository, SensorMeasurementRepository>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
builder.Services.AddScoped<IMeasurementTypeRepository, MeasurementTypeRepository>();

builder.Services.AddScoped<IControllerService, ControllerService>();
builder.Services.AddScoped<ISensorService, SensorService>();
builder.Services.AddScoped<ISensorMeasurementService, SensorMeasurementService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IMeasurementTypeService, MeasurementTypeService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(AngularDevCorsPolicy);

app.UseHttpsRedirection();

app.MapControllers();

app.Run();