using FlightInfoProducer.Models;
using FlightInfoProducer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FlightInfoProducer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightInfoController : ControllerBase
    {
        private readonly ProducerService _producerService;

        public FlightInfoController(ProducerService producerService)
        {
            _producerService = producerService;
        }

        [HttpPost]
        public async Task<IActionResult> UpdateFlightInfo([FromBody] FlightInfoRequest request)
        {
            var message = JsonSerializer.Serialize(request);

            await _producerService.ProduceAsync("FlightInfo", message);

            return Ok("FlightInfo Updated Successfully.");
        }
    }
}