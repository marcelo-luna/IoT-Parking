using System;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ParkingHub;
using System.Configuration;
using ParkingHub.Configuration;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ParkingDevice.Simulator;
using Interfaces.ParkingDevice;

namespace parking
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Init device");
            IHost host = CreateHostBuilder(args)
                .ConfigureAppConfiguration((hostingContext, configuration) =>
                {
                    IHostEnvironment env = hostingContext.HostingEnvironment;
                    configuration
                       .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
                       //.AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true);
                }).ConfigureServices((context, services) =>
                {
                    services.AddSingleton(new DeviceConfiguration(
                        context.Configuration.GetSection("IotHub").GetValue<string>("DeviceKey"),
                        context.Configuration.GetSection("IotHub").GetValue<string>("DeviceId"),
                        context.Configuration.GetSection("IotHub").GetValue<string>("IotHubHostName")));
                    services.AddHttpClient();
                    services.AddSingleton(new ParkingService(services.BuildServiceProvider().GetRequiredService<DeviceConfiguration>(), services.BuildServiceProvider().GetRequiredService<System.Net.Http.HttpClient>() ));
                    services.AddSingleton<IGateSensor, GateSensorSimulator>();
                })
                .Build();

            await host.RunAsync();
            //VerifyParking().Wait();
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args);
    }
}
