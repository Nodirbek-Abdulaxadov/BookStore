using BookStore.InventoryService.Data;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddTransient<ICategoryInterface, CategoryRepository>();
builder.Services.AddTransient<IBookInterface, BookRepository>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(st => new InventoryDatabase(
        builder.Configuration.GetConnectionString("MongoDB")!,
        builder.Configuration.GetConnectionString("Database")!
    ));


builder.Services.Configure<ConfigurationOptions>(builder.Configuration.GetSection("RedisCacheOptions"));
builder.Services.AddStackExchangeRedisCache(setupAction =>
{
    setupAction.Configuration = builder.Configuration.GetConnectionString("Redis");
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
