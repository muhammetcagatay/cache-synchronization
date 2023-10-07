using memory_cache_synchronization.Helpers;
using memory_cache_synchronization.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

builder.Services.AddSingleton<ICacheProvider, InMemoryCacheProvider>();
builder.Services.AddSingleton<IRedisProvider,RedisProvider>();
builder.Services.AddSingleton<ICacheSynchronizeProvider, CacheSynchronizeProvider>();
builder.Services.AddScoped<IBalanceService, BalanceService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

CacheSynchronizeProvider cacheSynchronizeProvider = app.Services.GetRequiredService<ICacheSynchronizeProvider>() as CacheSynchronizeProvider;
await cacheSynchronizeProvider.SubscribeCacheSyncChannel();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
