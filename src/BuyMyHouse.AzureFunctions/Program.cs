using BuyMyHouse.Domain.Repositories;
using BuyMyHouse.Domain.Services;
using BuyMyHouse.Infrastructure.Database;
using BuyMyHouse.Infrastructure.Repositories;
using BuyMyHouse.Infrastructure.Services;
using BuyMyHouse.Infrastructure.Storage;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

// Connection strings
string storageConnection = "UseDevelopmentStorage=true";
string sqlConnection = "Server=localhost,1433;Database=BuyMyHouseDB;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;";

// EF Core
builder.Services.AddDbContext<BuyMyHouseDbContext>(options =>
    options.UseSqlServer(sqlConnection));

// Azure Storage Services
builder.Services.AddSingleton(new BlobService(storageConnection));
builder.Services.AddSingleton(new QueueService(storageConnection));
builder.Services.AddSingleton(new TableService(storageConnection));
builder.Services.AddSingleton<EmailService>();

builder.Services.AddScoped<MortgageService>();
builder.Services.AddScoped<IMortgageApplicationRepository, MortgageApplicationRepository>();
builder.Services.AddDbContext<BuyMyHouseDbContext>(options =>
    options.UseSqlServer(Environment.GetEnvironmentVariable("BuyMyHouseDbConnection")));


var app = builder.Build();

app.Run();