using ConferenceBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConferenceBooking.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Hall> Halls { get; set; }
    public DbSet<RoomService> RoomServices { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<BookingServiceItem> BookingServiceItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Hall>()
            .Property(h => h.BasePricePerHour).HasPrecision(18, 2);
        modelBuilder.Entity<RoomService>()
            .Property(s => s.Price).HasPrecision(18, 2);
        modelBuilder.Entity<Booking>()
            .Property(b => b.TotalCost).HasPrecision(18, 2);
        
        modelBuilder.Entity<RoomService>()
            .HasOne(s => s.Hall)
            .WithMany(h => h.Services)
            .HasForeignKey(s => s.HallId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Hall)
            .WithMany(h => h.Bookings)
            .HasForeignKey(b => b.HallId)
            .OnDelete(DeleteBehavior.Cascade);

        
        modelBuilder.Entity<BookingServiceItem>()
            .HasOne(bs => bs.Booking)
            .WithMany(b => b.BookingServices)
            .HasForeignKey(bs => bs.BookingId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<BookingServiceItem>()
            .HasOne(bs => bs.RoomService)
            .WithMany()
            .HasForeignKey(bs => bs.RoomServiceId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}