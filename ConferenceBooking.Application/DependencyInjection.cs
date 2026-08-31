using System.Reflection;
using ConferenceBooking.Application.Interfaces.Service;
using ConferenceBooking.Application.Pricing;
using ConferenceBooking.Application.Service;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ConferenceBooking.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IHallService, HallService>();
        services.AddScoped<IBookingService, BookingService>();

        services.AddSingleton<PricingCalculator>();
        
        services.AddScoped<IReportService, ReportService>();

        services.AddAutoMapper(cfg 
            => cfg.AddMaps(Assembly.GetExecutingAssembly()));
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}