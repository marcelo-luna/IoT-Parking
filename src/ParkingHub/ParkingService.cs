using Microsoft.Azure.Devices.Client;
using ParkingHub.Configuration;
using ParkingModel;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ParkingHub
{
    public class ParkingService : IParkingService
    {
        private DeviceConfiguration _deviceConfiguration;

        public ParkingService(DeviceConfiguration deviceConfiguration)
        {
            _deviceConfiguration = deviceConfiguration;
            VerifyParking().Wait();
        }

        public async Task VerifyParking()
        {

            var deviceAuthentication = new DeviceAuthenticationWithRegistrySymmetricKey(_deviceConfiguration.DeviceId, _deviceConfiguration.DeviceKey);

            DeviceClient deviceClient = DeviceClient.Create(_deviceConfiguration.IotHubHostName, deviceAuthentication, TransportType.Mqtt);
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

                var telemetryDataPoint = new Parking
                {
                    MessageId = Guid.NewGuid().ToString(),
                    DeviceId = _deviceConfiguration.DeviceId,
                    Action = currentAction == 0 ? "New exit" : "New enter",
                    CarsParking = totalParking
                };


                string messageString = JsonSerializer.Serialize(telemetryDataPoint);
                Message message = new Message(Encoding.ASCII.GetBytes(messageString));
                message.Properties.Add("parking", (currentAction > 40) ? "true" : "false");

                await deviceClient.SendEventAsync(message);
                Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, messageString);

                await Task.Delay(1000);
            }
        }
    }
}
