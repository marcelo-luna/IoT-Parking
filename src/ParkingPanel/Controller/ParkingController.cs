using Microsoft.AspNetCore.Mvc;

namespace ParkingPanel.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParkingController : ControllerBase
    {
        [HttpPost]
        public ActionResult ParkingPost()
        {
            return Ok();
        }

        [HttpGet]
        public ActionResult ParkingGet()
        {
            return Ok();
        }
    }
}
