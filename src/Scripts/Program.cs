using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using QuickShop.DependencyInjection;
using QuickShop.DependencyInjection.Repository;
using QuickShop.Repository.Mongo;
using QuickShop.Scripts.Mongo;

namespace QuickShop.Scripts
{
    /// <summary>
    /// This class contains bootstrapper class, dependency injection and rendering logic.
    /// If you want to update the list of scripts or add services to DI, <see cref="Startup"/>
    /// </summary>
    class Program
    {
        static async Task Main()
        {
            var serviceProvider = BuildServiceProvider();

            var scripts = serviceProvider.GetRequiredService<IScript[]>();

            await PromptAndRunScript(scripts);
        }

        private static async Task PromptAndRunScript(IScript[] scripts)
        {
            Console.WriteLine("Choose a script to run");
            Console.WriteLine("  0) Cancel & exit");

            for (int i = 0; i < scripts.Length; i++)
            {
                Console.WriteLine($"  {i + 1}) {scripts[i].GetType().Name}");
            }

            string input = Console.ReadLine();

            if (String.IsNullOrWhiteSpace(input) || !int.TryParse(input, out int inputNumber) || inputNumber < 0 || inputNumber > scripts.Length)
            {
                Console.WriteLine($"Invalid input: {input}");
                return;
            }

            if (inputNumber == 0)
                return;

            await scripts[inputNumber - 1].Run();
            Console.WriteLine("Completed!");
        }

        private static IServiceProvider BuildServiceProvider()
        {
            // Configuration
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.user.json", optional: true);

            var configuration = configurationBuilder.Build();

            // Create startup
            var startup = new Startup(configuration);

            var scriptTypes = startup.GetScripts();

            // Service collection
            var serviceCollection = new ServiceCollection();

            startup.ConfigureServices(serviceCollection);

            // Automatically register scripts and list of scripts
            foreach (var scriptType in scriptTypes)
            {
                serviceCollection.AddTransient(scriptType);
            }

            serviceCollection.AddTransient<IScript[]>(sp =>
            {
                return scriptTypes.Select(scriptType => (IScript) sp.GetRequiredService(scriptType)).ToArray();
            });
            
            return serviceCollection.BuildServiceProvider();
        }
    }
}
