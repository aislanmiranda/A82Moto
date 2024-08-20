using Application.Implementation;
using Application.Interface;
using Application.Mapper;
using FluentValidation.AspNetCore;
using Infra.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ApplicationModule
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services
            .AddApplicationServices()
            .AddAutoMapper()
            .AddValidationField();

        return services;
    }

    private static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IDeliveryManService, DeliveryManService>();
        services.AddScoped<IMotorcycleService, MotorcycleService>();
        services.AddScoped<IPlanService, PlanService>();
        services.AddScoped<IRentService, RentService>();
        services.AddScoped<IRabbitMQService, RabbitMQService>();

        return services;
    }

    private static IServiceCollection AddAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MotocycleMapper));
        services.AddAutoMapper(typeof(DeliveryManMapper));
        services.AddAutoMapper(typeof(PlanMapper));
        services.AddAutoMapper(typeof(RentMapper));
        services.AddAutoMapper(typeof(NotificationMapper));

        return services;
    }

    private static IServiceCollection AddValidationField(this IServiceCollection services)
    {
        services.AddFluentValidation(config =>
        {
            config.RegisterValidatorsFromAssembly(typeof(ApplicationModule).Assembly);
        });

        return services;
    }
}

