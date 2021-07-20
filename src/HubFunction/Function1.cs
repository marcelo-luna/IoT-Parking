using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace HubFunction
{
    public static class Function1
    {
        [Function("Function1")]
        public static void Run([EventHubTrigger("parking", Connection = "CONNECTION_STRING")] string[] input, FunctionContext context)
        //public static async Task Run([EventHubTrigger("parking", Connection = "CONNECTION_STRING")] EventData[] events, ILogger log)
        {
            var exceptions = new List<Exception>();

            var data = context.BindingContext.BindingData;

            var eventData = context.BindingContext.BindingData.Values;
            var times = JsonSerializer.Deserialize<List<DateTime>>(data["EnqueuedTimeUtcArray"].ToString());

            for (int i = 0; i < input.Length; i++)
            {
                Console.WriteLine(input[i]);
                var enqueuedTimeUtc = times[i];//corresponding event enqueue time for message
            }

            if (exceptions.Count > 1)
                throw new AggregateException(exceptions);

            if (exceptions.Count == 1)
                throw exceptions.Single();
        }
    }
}
