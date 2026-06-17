using Microsoft.EntityFrameworkCore;
using ZenPay.Core.Data; // For ZenPayDbContext
using ZenPay.Core.Models; // For Transaction model
using Scalar.AspNetCore;

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
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

// 4. Endpoints
app.MapGet("/", () => Results.Redirect("/scalar")).ExcludeFromDescription();

app.MapGet("/transactions", async (ZenPayDbContext db) =>
    await db.Transactions.ToListAsync())
    .WithName("GetTransactions");

app.MapGet("/transactions/{id}", async (Guid id, ZenPayDbContext db) =>
    await db.Transactions.FindAsync(id)
        is Transaction transaction
            ? Results.Ok(transaction)
            : Results.NotFound())
    .WithName("GetTransactionById");

app.MapPost("/transactions", async (Transaction transaction, ZenPayDbContext db) =>
{
    transaction.Id = Guid.NewGuid(); // ensure a new ID
    transaction.Status = ZenPay.Core.ENUM.TransactionStatus.Pending.ToString();
    transaction.TransactionDateTime = DateTimeOffset.UtcNow;
    
    // Generate a mock transaction number if not provided
    if (string.IsNullOrEmpty(transaction.TransactionNumber)) 
    {
        transaction.TransactionNumber = $"TXN-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString().Substring(0, 4)}";
    }

    db.Transactions.Add(transaction);
    await db.SaveChangesAsync();

    return Results.Created($"/transactions/{transaction.Id}", transaction);
})
.WithName("CreateTransaction");

app.MapPost("/transactions/{id}/process", async (Guid id, ZenPayDbContext db) =>
{
    var transaction = await db.Transactions.FindAsync(id);
    if (transaction is null) return Results.NotFound();

    if (transaction.Status != ZenPay.Core.ENUM.TransactionStatus.Pending.ToString())
    {
        return Results.BadRequest("Only pending transactions can be processed.");
    }

    // Mock processing logic: 80% success, 20% fail
    var random = new Random();
    if (random.Next(100) < 80)
    {
        transaction.Status = ZenPay.Core.ENUM.TransactionStatus.Captured.ToString();
    }
    else
    {
        transaction.Status = ZenPay.Core.ENUM.TransactionStatus.Failed.ToString();
        transaction.ErrorCode = "ERR_INSUFFICIENT_FUNDS";
    }

    await db.SaveChangesAsync();
    return Results.Ok(transaction);
})
.WithName("ProcessTransaction");

app.Run();