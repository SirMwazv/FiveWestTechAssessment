using System.Net;
using System.Net.Security;
using OrderbookAPI.Services;
using OrderBookAPI.Services;

ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

var builder = WebApplication.CreateBuilder(args);

// Bind the application to port 5002
builder.WebHost.UseUrls("http://0.0.0.0:5002");
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(5002);
});
// Add services to the container
builder.Services.AddControllers();

// Register the InMemoryOrderBook and OrderbookService in the DI container
builder.Services.AddSingleton<InMemoryOrderBook>();

// Add IConfiguration to your services (appsettings.json configuration)
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});
// Register OrderbookService as a hosted service
builder.Services.AddHostedService<OrderbookService>();

// Register HttpClientFactory (needed to connect to Binance)
builder.Services.AddHttpClient();  

// Add Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Step 2: Build the app
var app = builder.Build();

// Step 3: Configure the HTTP request pipeline (AFTER building the app)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
app.Use(async (context, next) =>
{
    Console.WriteLine($"Request: {context.Request.Method} {context.Request.Path}");
    await next.Invoke();
});
app.UseCors("AllowAll");

app.UseRouting();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();

    // Handle OPTIONS requests for preflight
    endpoints.MapMethods("/api/{**path}", new[] { "OPTIONS" }, async context =>
    {
        context.Response.Headers.Add("Access-Control-Allow-Origin", "http://localhost:3000");
        context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE");
        context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization");
        await context.Response.CompleteAsync();
    });
});


app.MapControllers();
app.MapGet("/health", () => "Healthy");

app.Run();