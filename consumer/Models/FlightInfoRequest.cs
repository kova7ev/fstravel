using System.ComponentModel.DataAnnotations;

namespace FlightInfoConsumer.Models
{
    public class FlightInfoRequest
    {


        // Информация о рейсе
        [Key]
        public required int FlightId { get; set; }
        public string FlightNumber { get; set; } = string.Empty;
        public string Airline { get; set; } = string.Empty;
        public string ArrivalAirport { get; set; } = string.Empty;
        public string Terminal { get; set; } = string.Empty;
        public string FlightStatus { get; set; } = string.Empty;
    }
}