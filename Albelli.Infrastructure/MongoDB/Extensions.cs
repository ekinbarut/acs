using System;
using Albelli.Abstraction.Entity;
using Albelli.Abstraction.Service;
using Albelli.Infrastructure.Settings;
using Consul;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Albelli.Infrastructure.MongoDB
{
    public static class Extensions
    {
        public static IServiceCollection AddMongo(this IServiceCollection services)
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

            services.AddSingleton(a =>
            {
                var configuration = a.GetService<IConfiguration>();
                var serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
                var mongoDbSettings = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
                var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
                return mongoClient.GetDatabase(serviceSettings.ServiceName);
            });

            return services;
        }

        public static IServiceCollection AddMongoRepository<T>(this IServiceCollection services, string collectionName) where T : IEntity
        {
            services.AddSingleton<IRepository<T>>(a =>
            {
                var database = a.GetService<IMongoDatabase>();
                return new MongoRepository<T>(database, collectionName);
            });
            return services;
        }
        
       
        
        public static IServiceCollection AddConsul(this IServiceCollection services)  
        {              
            services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>  
            {  
                //consul address  
                var configuration = p.GetService<IConfiguration>();
                var address = configuration["Consul:Host"];  
                consulConfig.Address = new Uri(address);  
            }, null, handlerOverride =>  
            {  
                //disable proxy of httpclienthandler  
                handlerOverride.Proxy = null;  
                handlerOverride.UseProxy = false;  
            }));  
            return services;  
        }  
    }
}
