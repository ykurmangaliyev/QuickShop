using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MongoDB.Driver;
using QuickShop.Extensions.Configuration;
using QuickShop.Repository.Mongo;
using QuickShop.Repository.Mongo.Accounts;
using QuickShop.Scripts.Mongo;

namespace QuickShop.Scripts
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = ConfigurationExtensions.LoadConfigurationUserFile("Scripts");

            string connectionString = configuration.ConnectionStrings.ConnectionStrings["Mongo"].ConnectionString;
            string databaseName = configuration.AppSettings.Settings["DatabaseName"].Value;

            var clientWrapper = new MongoClientWrapper(new MongoClient(connectionString), databaseName);

            var scripts = new IScript[]
            {
                new CreateFirstUser(clientWrapper),
            };

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
    }
}
