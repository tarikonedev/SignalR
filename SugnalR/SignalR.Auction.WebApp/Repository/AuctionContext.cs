using Microsoft.EntityFrameworkCore;
using SignalR.Auction.WebApp.Model;

namespace SignalR.Auction.WebApp.Repository;

public class AuctionContext : DbContext
{
    public DbSet<Lot> Lots { get; set; }

    public string DbPath { get; }

    public AuctionContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);

        DbPath = System.IO.Path.Join(path, "blogging.db");

        Database.Migrate();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite($"Data Source={DbPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Lot>().ToTable("Lots");

        base.OnModelCreating(modelBuilder);
    }
}
