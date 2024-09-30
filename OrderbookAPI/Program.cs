using System.Net;
using System.Net.Security;
using OrderbookAPI.Services;
using OrderBookAPI.Services;

ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Register the InMemoryOrderBook and OrderbookService in the DI container
builder.Services.AddSingleton<InMemoryOrderBook>(); // In-memory orderbook service
// Register OrderbookService as a hosted service
builder.Services.AddHostedService<OrderbookService>();

// Add IConfiguration to your services
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Add CORS service to allow any origin, method, and header
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policyBuilder =>
    {
        policyBuilder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Add Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline

    app.UseSwagger();
    app.UseSwaggerUI();


// Enable CORS globally
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();