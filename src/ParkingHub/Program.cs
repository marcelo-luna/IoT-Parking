using System;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using System.Text.Json;
using System.Text;

namespace parking
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Init device");
            VerifyParking().Wait();
        }

        public static async Task VerifyParking()
        {
            string deviceKey = "";
            string deviceId = "";
            string iotHubHostName = "";
            var deviceAuthentication = new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey);

            DeviceClient deviceClient = DeviceClient.Create(iotHubHostName, deviceAuthentication, TransportType.Mqtt);
            var rand = new Random();
            int totalParking = 0;

            while (true)
            {
                int currentAction = rand.Next(0, 2);

                if (currentAction == 0 && totalParking == 0) continue;

                if (currentAction == 1)
                    totalParking++;
                else
                    totalParking--;

                var telemetryDataPoint = new
                {
                    messageId = Guid.NewGuid(),
                    deviceId = deviceId,
                    action = currentAction == 0 ? "New exit" : "New enter",
                    carsParking = totalParking
                };


                string messageString = JsonSerializer.Serialize(telemetryDataPoint);
                Message message = new Message(Encoding.ASCII.GetBytes(messageString));
                message.Properties.Add("parking", (currentAction > 40) ? "true" : "false");

                await deviceClient.SendEventAsync(message);
                Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, messageString);

                await Task.Delay(3000);
            }
        }
    }
}
