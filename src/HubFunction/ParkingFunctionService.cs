using System.Net.Http;
using System.Threading.Tasks;

namespace HubFunction
{
    public class ParkingFunctionService
    {
        public async Task UpdateFrontEnd(int totalParking)
        {
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), $"http://localhost:8897/api/parking/{totalParking}"))
                {
                    var response = await httpClient.SendAsync(request);
                }
            }
        } 
    }
}
