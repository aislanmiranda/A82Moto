using Infra.Mongo;
using Infra.Repositories.Implementation;
using Domain.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;
using MassTransit;
using Infra.Messaging;

namespace Infra;

public static class InfraModule
{

    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services
            .AddMongo()
            .AddRabbitMQ()
            .AddRepositories();

        return services;
    }

    public static IServiceCollection AddMongo(this IServiceCollection services)
    {
        services.AddSingleton<MongoDbOptions>(sp => {
            var configuration = sp.GetService<IConfiguration>();
            var options = new MongoDbOptions();

            configuration?.GetSection("Mongo").Bind(options);

            return options;
        });

        services.AddSingleton<IMongoClient>(sp => {
            var configuration = sp.GetService<IConfiguration>();
            var options = sp.GetService<MongoDbOptions>();

            var client = new MongoClient(options.ConnectionString);

            var db = client.GetDatabase(options.Database);
            var dbSeed = new DbSeed(db);
            dbSeed.Populate();

            return client;
        });

        services.AddTransient(sp => {
            BsonDefaults.GuidRepresentation = GuidRepresentation.Standard;

            var options = sp.GetService<MongoDbOptions>();
            var mongoClient = sp.GetService<IMongoClient>();

            var db = mongoClient?.GetDatabase(options?.Database);

            return db;
        });

        return services;
    }

    public static IServiceCollection AddRabbitMQ(this IServiceCollection services)   
        => services.AddScoped<IRabbitMQService, RabbitMQService>();

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();
        services.AddScoped<IDeliveryManRepository, DeliveryManRepository>();
        services.AddScoped<IPlanRepository, PlanRepository>();
        services.AddScoped<IRentRepository, RentRepository>();

        return services;
    }
}


