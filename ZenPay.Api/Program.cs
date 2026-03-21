using Microsoft.EntityFrameworkCore;
using ZenPay.Core.Data; // For ZenPayDbContext
using ZenPay.Core.Models; // For Transaction model

var builder = WebApplication.CreateBuilder(args);

// 1. Add services to the container.
builder.Services.AddOpenApi();

// 2. Register SQLite DbContext
// This creates a file named "ZenPay.db" in your project folder
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? "Data Source=ZenPay.db";

builder.Services.AddDbContext<ZenPayDbContext>(options =>
    options.UseSqlite(connectionString));

var app = builder.Build();

// 3. Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// 4. Your First "Practice" Endpoint: Get all transactions
app.MapGet("/transactions", async (ZenPayDbContext db) =>
    await db.Transactions.ToListAsync())
    .WithName("GetTransactions");

app.Run();