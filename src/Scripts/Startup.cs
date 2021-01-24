using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuickShop.DependencyInjection;
using QuickShop.DependencyInjection.Repository;
using QuickShop.Repository.Mongo.Configuration;
using QuickShop.Scripts.Mongo;

namespace QuickShop.Scripts
{
    internal class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public Type[] GetScripts()
        {
            return new[]
            {
                typeof(CreateFirstUser),
            };
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<MongoOptions>(Configuration.GetSection(MongoOptions.Mongo));

            services.AddQuickShop()
                .AddMongoRepository();
        }
    }
}
