using FlightInfoConsumer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightInfoConsumer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightInfoController : ControllerBase
    {
        private readonly ILogger _logger;

        public FlightInfoController(ILogger logger)
        {
            _logger = logger;
        }
        [HttpPost]
        public IActionResult ProcessFlightInfo([FromBody] FlightInfoRequest request)
        {
            return Ok("FlightInfo processed successfully.");
        }
    }
}