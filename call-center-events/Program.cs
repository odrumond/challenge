using Data.Data.Interfaces;
using Data.Data.Helper;
using Microsoft.EntityFrameworkCore;
using Data.Data.Contexts.Mongo;
using Data.Data.Contexts.Postgres;
using Services.Services.Implementations;
using Services.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var database = builder.Configuration.GetValue("Database", "Postgres") ?? throw new Exception("No found database");

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IEventsService, EventsService>();
builder.Services.AddScoped<IAgentsService, AgentsService>();



builder.Services.AddScoped<ICallCenterDbContext>((serviceProvider) =>
{
    if (database == "MongoDB")
        return serviceProvider.GetService<MongoCallCenterDbContext>();

    return serviceProvider.GetService<PostgresCallCenterDbContext>();
});

builder.Services.AddSingleton<ITimeService, TimeService>();

builder.Services.AddDbContext<PostgresCallCenterDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));
builder.Services.AddDbContext<MongoCallCenterDbContext>(options => options.UseMongoDB(builder.Configuration.GetConnectionString("MongoDB") ?? "", "dbuser"));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ICallCenterDbContext>();
    await DbHelper.InitializeAsync(db);
}

app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
