using Converter.Volume;
using Converter.Weight;
using ConverterAPI.services;
using ConverterAPI.setup;
using FeatureToggle;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IWeightConverter, WeightConverter>();
builder.Services.AddSingleton<IVolumeConverter, VolumeConverter>();

builder.Configuration.AddEnvironmentVariables();
builder.Configuration["DbSettings:ConnectionString"] = Environment.GetEnvironmentVariable("DB_CON_STR");

builder.Services.Configure<SqlDbConfig>(builder.Configuration.GetSection("DbSettings"));
builder.Services.AddSingleton<SqlService>();

builder.Services.AddSingleton<IFeatureToggleService, FeatureToggleService>();


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin() 
            .AllowAnyMethod() 
            .AllowAnyHeader(); 
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.MapControllers();

app.Run();