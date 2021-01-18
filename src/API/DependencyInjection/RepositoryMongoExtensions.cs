using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using QuickShop.Domain.Accounts.Model.UserAggregate;
using QuickShop.Repository.Mongo;
using QuickShop.Repository.Mongo.Accounts;

namespace API.DependencyInjection
{
    public static class RepositoryMongoExtensions
    {
        [SuppressMessage("ReSharper", "RedundantTypeArgumentsOfMethod")]
        public static void AddMongoRepository(this IServiceCollection services, string connectionString, string databaseName)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.AddSingleton<MongoClient>(_ => new MongoClient(connectionString));
            services.AddSingleton<MongoClientWrapper>(s => new MongoClientWrapper(s.GetService<MongoClient>(), databaseName));

            services.AddTransient<IUserRepository, MongoUserRepository>();
        }
    }
}
