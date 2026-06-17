using Microsoft.EntityFrameworkCore;
using ZenPay.Core.Data;
using Scalar.AspNetCore;
using ZenPay.Core.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Add services to the container.
// Register Controllers (This tells ASP.NET to look for classes inheriting from ControllerBase)
builder.Services.AddControllers();

builder.Services.AddOpenApi();

// Register our new Transaction Service for Dependency Injection!
// 'AddScoped' means a new instance of TransactionService is created for every single HTTP request.
builder.Services.AddScoped<ITransactionService, TransactionService>();

// 2. Register SQLite DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? "Data Source=ZenPay.db";

builder.Services.AddDbContext<ZenPayDbContext>(options =>
    options.UseSqlite(connectionString));

var app = builder.Build();

// 3. Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

// 4. Endpoints
// We tell ASP.NET to scan our project for Controllers and map their routes
app.MapControllers();

// A simple redirect for the root URL so the scalar UI still loads easily
app.MapGet("/", () => Results.Redirect("/scalar")).ExcludeFromDescription();

app.Run();