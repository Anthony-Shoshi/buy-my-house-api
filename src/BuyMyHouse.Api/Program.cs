using BuyMyHouse.Domain.Repositories;
using BuyMyHouse.Infrastructure.Database;
using BuyMyHouse.Infrastructure.Repositories;
using BuyMyHouse.Domain.Services;
using Microsoft.EntityFrameworkCore;
using BuyMyHouse.Infrastructure.Storage;
using BuyMyHouse.Infrastructure.Seed;

var builder = WebApplication.CreateBuilder(args);

// --- DATABASE ---
var sqlConn = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(sqlConn))
{
    // optional: fail fast with clearer message
    Console.WriteLine("WARNING: DefaultConnection is not configured.");
}
builder.Services.AddDbContext<BuyMyHouseDbContext>(options =>
    options.UseSqlServer(sqlConn));

// --- MVC/Swagger ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- Repositories / Domain services ---
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IMortgageApplicationRepository, MortgageApplicationRepository>();
builder.Services.AddScoped<MortgageService>();

// --- Storage connection selection ---
string blobConn;
string queueConn;
string tableConn;

if (builder.Environment.IsDevelopment())
{
    // local dev defaults (Azurite)
    blobConn = builder.Configuration.GetConnectionString("BlobStorage") ?? "UseDevelopmentStorage=true";
    queueConn = builder.Configuration.GetConnectionString("QueueStorage") ?? "UseDevelopmentStorage=true";
    tableConn = builder.Configuration.GetConnectionString("TableStorage") ?? "UseDevelopmentStorage=true";
}
else
{
    // Read from Configuration.GetConnectionString or from AppSettings fallback
    blobConn = builder.Configuration.GetConnectionString("BlobStorage")
               ?? builder.Configuration["BlobStorage"]
               ?? throw new InvalidOperationException("BlobStorage connection not configured");
    queueConn = builder.Configuration.GetConnectionString("QueueStorage")
               ?? builder.Configuration["QueueStorage"]
               ?? throw new InvalidOperationException("QueueStorage connection not configured");
    tableConn = builder.Configuration.GetConnectionString("TableStorage")
               ?? builder.Configuration["TableStorage"]
               ?? throw new InvalidOperationException("TableStorage connection not configured");
}

builder.Services.AddSingleton(new BlobService(blobConn));
builder.Services.AddSingleton(new QueueService(queueConn));
builder.Services.AddSingleton(new TableService(tableConn));

var app = builder.Build();

// Run migrations & seed (only if connection exists)
using (var scope = app.Services.CreateScope())
{
    var sp = scope.ServiceProvider;
    try
    {
        var db = sp.GetRequiredService<BuyMyHouseDbContext>();
        if (!string.IsNullOrWhiteSpace(sqlConn))
        {
            await db.Database.MigrateAsync();
            await DbInitializer.SeedAsync(db);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Migration/Seed error: " + ex.Message);
        throw;
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
