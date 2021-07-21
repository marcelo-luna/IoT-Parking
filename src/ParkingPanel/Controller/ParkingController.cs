using Microsoft.AspNetCore.Mvc;
using ParkingPanel.Services;

namespace ParkingPanel.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParkingController : ControllerBase
    {
        private ParkingService _parkingService;
        public ParkingController(ParkingService parkingService)
        {
            _parkingService = parkingService;
        }

        [HttpPost("{parkingCount}")]
        public ActionResult ParkingPost(int parkingCount)
        {
            try
            {
                _parkingService.UpdateParking(parkingCount);
            }
            catch (System.Exception)
            {

                return StatusCode(500);
            }
            return Ok();
        }

        [HttpGet]
        public ActionResult ParkingGet()
        {

            return Ok(_parkingService.ParkingCount);
        }
    }
}
