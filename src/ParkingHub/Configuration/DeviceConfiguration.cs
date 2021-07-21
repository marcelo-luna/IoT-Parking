namespace ParkingHub.Configuration
{
    public class DeviceConfiguration
    {
        public string DeviceKey { get; private set; }
        public string DeviceId { get; private set; }
        public string IotHubHostName { get; private set; }

        public DeviceConfiguration(string deviceKey, string deviceId, string iotHubHostName)
        {
            DeviceKey = deviceKey;
            DeviceId = deviceId;
            IotHubHostName = iotHubHostName;
        }
    }
}
