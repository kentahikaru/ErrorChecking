using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ErrorChecking.Configuration;
using ErrorChecking.Services;

namespace ErrorChecking
{
    class Program
    {
        static async Task Main(string[] args)
        {
             try
            {
                Console.WriteLine("Program starting");

                await Host.CreateDefaultBuilder(args)
                    .ConfigureAppConfiguration((hostingContext, config) =>
                    {
                        config.AddEnvironmentVariables();
                    })
                    .ConfigureServices((hostContext, services) => {
                        services.AddHostedService<ConsoleHostedService>();
                        services.AddOptions<ErrorCheckingConfiguration>().Bind(hostContext.Configuration.GetSection("ErrorCheckingConfiguration"));
                    })
                    .RunConsoleAsync();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;
            }
            finally
            {
            }
        }
    }
}
