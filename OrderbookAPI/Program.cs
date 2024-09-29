using System.Net;
using System.Net.Security;

ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


var builder = WebApplication.CreateBuilder(args);


// Add services to the container
builder.Services.AddControllers();

// Add the OrderbookService to the DI container
builder.Services.AddSingleton<OrderbookAPI.Services.OrderbookService>();


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
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable CORS globally
app.UseCors("AllowAll");

// app.UseHttpsRedirection();

app.UseAuthorization();

// Map controllers
app.MapControllers();

app.Run();