using ConferenceBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConferenceBooking.Infrastructure.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (await context.Halls.AnyAsync())
            return;   
        var halls = new List<Hall>
        {
            new Hall
            {
                Name = "Зал А",
                Capacity = 50,
                BasePricePerHour = 2000,
                Services = new List<RoomService>
                {
                    new() { Name = "Проєктор", Price = 500 },
                    new() { Name = "Wi-Fi", Price = 300 }
                }
            },
            new Hall
            {
                Name = "Зал B",
                Capacity = 100,
                BasePricePerHour = 3500,
                Services = new List<RoomService>
                {
                    new() { Name = "Проєктор", Price = 500 },
                    new() { Name = "Звук", Price = 700 }
                }
            },
            new Hall
            {
                Name = "Зал C",
                Capacity = 30,
                BasePricePerHour = 1500,
                Services = new List<RoomService>
                {
                    new() { Name = "Wi-Fi", Price = 300 }
                }
            }
        };
        await context.Halls.AddRangeAsync(halls);
        await context.SaveChangesAsync();
    }
}