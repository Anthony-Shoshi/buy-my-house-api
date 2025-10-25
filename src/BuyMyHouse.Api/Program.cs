using BuyMyHouse.Domain.Repositories;
using BuyMyHouse.Infrastructure.Database;
using BuyMyHouse.Infrastructure.Repositories;
using BuyMyHouse.Domain.Services;
using Microsoft.EntityFrameworkCore;
using BuyMyHouse.Infrastructure.Storage;
using BuyMyHouse.Infrastructure.Seed;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BuyMyHouseDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IMortgageApplicationRepository, MortgageApplicationRepository>();

builder.Services.AddScoped<MortgageService>();

string storageConnection = "UseDevelopmentStorage=true"; // for Azurite
builder.Services.AddSingleton(new BlobService(storageConnection));
builder.Services.AddSingleton(new QueueService(storageConnection));
builder.Services.AddSingleton(new TableService(storageConnection));

var app = builder.Build();

// using (var scope = app.Services.CreateScope())
// {
//     var db = scope.ServiceProvider.GetRequiredService<BuyMyHouseDbContext>();
//     await db.Database.MigrateAsync();  // ensure DB schema is created
//     await DbInitializer.SeedAsync(db); // seed your data
// }

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
