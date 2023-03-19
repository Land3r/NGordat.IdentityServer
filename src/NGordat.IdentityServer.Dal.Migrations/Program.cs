using CommandLine;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NGordat.IdentityServer.Dal;
using NGordat.IdentityServer.Dal.Extensions;
using NGordat.IdentityServer.Dal.Migrations;
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();

        var typedArgs = Parser.Default.ParseArguments<Options>(args)
            .WithParsed<Options>(async options => {
                Console.WriteLine("Starting");

                if (options == null) return;

                IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.RegisterDbContexts(configuration);

                    services.AddDuendeStores();
                }).Build();

                if (options.Migrate)
                {
                    Console.WriteLine("Migration starting");
                    using (var scope = host.Services.CreateScope())
                    {
                        using (var context = scope.ServiceProvider.GetService<IdentityServerConfigurationDbContext>())
                        {
                            context.Database.Migrate();
                            Console.WriteLine($"{nameof(IdentityServerIdentityDbContext)} migrated successfully");
                        }

                        using (var context = scope.ServiceProvider.GetService<IdentityServerDataProtectionDbContext>())
                        {
                            context.Database.Migrate();
                            Console.WriteLine($"{nameof(IdentityServerDataProtectionDbContext)} migrated successfully");
                        }

                        using (var context = scope.ServiceProvider.GetService<IdentityServerPersistedGrantDbContext>())
                        {
                            context.Database.Migrate();
                            Console.WriteLine($"{nameof(IdentityServerPersistedGrantDbContext)} migrated successfully");
                        }

                        using (var context = scope.ServiceProvider.GetService<IdentityServerIdentityDbContext>())
                        {
                            context.Database.Migrate();
                            Console.WriteLine($"{nameof(IdentityServerIdentityDbContext)} migrated successfully");
                        }
                    }

                    Console.WriteLine("Migrations ended");
                }

                if (options.Seed)
                {
                    Console.WriteLine("Seed starting");

                    Console.WriteLine("Seed ended");
                }

                Console.WriteLine("Ending");
                await host.RunAsync();
            });
    }
}