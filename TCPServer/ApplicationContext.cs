namespace TCPServer;

using Microsoft.EntityFrameworkCore;
using TicketsLibrary;

public class ApplicationContext : DbContext
{
    public DbSet<Ticker> Tickers { get; set; } = null!;
    public DbSet<Price> Prices { get; set; } = null!;
    public DbSet<TodaysCondition> Conditions { get; set; } = null!;
    public ApplicationContext()
    {
        Database.EnsureCreatedAsync();
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source = ../../../tickerstest.sql");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Ticker>().HasKey(u => new {u.id, u.name });
        modelBuilder.Entity<Price>().HasOne(p => p.ticker).WithMany(q => q.Prices)
            .HasForeignKey(r => new { r.tickerId, r.tickerName });
        modelBuilder.Entity<TodaysCondition>().HasOne(p => p.ticker).WithOne(q => q.Condition)
            .HasForeignKey<TodaysCondition>(e => new { e.tickerId, e.tickerName });
    }
}