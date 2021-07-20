using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubFunction
{
    public class Program
    {
        static Task Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureAppConfiguration(configurationBuilder =>
                {
                    configurationBuilder.AddCommandLine(args);
                })
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices(services =>
                {
                    // Add Logging
                    services.AddLogging();

                    // Add HttpClient
                    //services.AddHttpClient();

                    // Add Custom Services
                    //services.AddSingleton<BlobStorageService>();
                    //services.AddSingleton<CloudBlobClient>(CloudStorageAccount.Parse(_storageConnectionString).CreateCloudBlobClient());
                })
                .Build();

            return host.RunAsync();
        }
    }
}