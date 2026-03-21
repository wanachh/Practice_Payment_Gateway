using Microsoft.EntityFrameworkCore;
using ZenPay.Core.Models;

namespace ZenPay.Core.Data;

public class ZenPayDbContext : DbContext
{
    public ZenPayDbContext(DbContextOptions<ZenPayDbContext> options) : base(options) { }

    // If you have 'using System.Transactions;' at the top, REMOVE IT.
    public DbSet<Transaction> Transactions { get; set; }
}