using Microsoft.Azure.Devices.Client;
using ParkingDevice.Simulator;
using ParkingHub.Configuration;
using ParkingModel;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ParkingHub
{
    public class ParkingService : IParkingService
    {
        private DeviceConfiguration _deviceConfiguration;
        private GateSensorSimulator _gateSensorSimulator;
        private HttpClient _httpClient;

        public ParkingService(DeviceConfiguration deviceConfiguration, HttpClient httpClient)
        {
            _deviceConfiguration = deviceConfiguration;
            _gateSensorSimulator = new GateSensorSimulator(40);
            _httpClient = httpClient;
            VerifyParking().Wait();
        }

        public async Task VerifyParking()
        {

            var deviceAuthentication = new DeviceAuthenticationWithRegistrySymmetricKey(_deviceConfiguration.DeviceId, _deviceConfiguration.DeviceKey);

            //coment test
            //DeviceClient deviceClient = DeviceClient.Create(_deviceConfiguration.IotHubHostName, deviceAuthentication, TransportType.Mqtt);

            _ = _gateSensorSimulator.GateSimulatorStart();

            while (true)
            {
                int totalParking = _gateSensorSimulator.TotalCarsParked;

                var telemetryDataPoint = new Parking
                {
                    MessageId = Guid.NewGuid().ToString(),
                    DeviceId = _deviceConfiguration.DeviceId,
                    //Action = currentAction == 0 ? "New exit" : "New enter",
                    CarsParking = _gateSensorSimulator.TotalCarsParked
                };


                string messageString = JsonSerializer.Serialize(telemetryDataPoint);
                Message message = new Message(Encoding.ASCII.GetBytes(messageString));
                message.Properties.Add("parking", (_gateSensorSimulator.TotalCarsParked > 40) ? "true" : "false");

                //coment test
                //await deviceClient.SendEventAsync(message);
                Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, messageString);
                await UpdateFrontEnd(_gateSensorSimulator.TotalCarsParked);
                await Task.Delay(1000);
            }
        }

        public async Task UpdateFrontEnd(int totalParking)
        {
            using (var request = new HttpRequestMessage(new HttpMethod("POST"), $"http://localhost:8897/api/parking/{totalParking}"))
            {
                var response = await _httpClient.SendAsync(request);
            }
        }
    }
}
