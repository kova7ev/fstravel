using Confluent.Kafka;
using FlightInfoConsumer.Models;
using Newtonsoft.Json;

namespace FlightInfoConsumer.Services
{
    public class ConsumerService : BackgroundService
    {
        private readonly IConsumer<Ignore, string> _consumer;
        private readonly ILogger<ConsumerService> _logger;

        private readonly IServiceScopeFactory _service;

        public ConsumerService(IConfiguration configuration, ILogger<ConsumerService> logger, IServiceScopeFactory scopeFactory)
        {

            _logger = logger;
            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"],
                GroupId = "FlightInfoConsumerGroup",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            _consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
            _service = scopeFactory;
            
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _consumer.Subscribe("FlightInfo");

            while (!stoppingToken.IsCancellationRequested)
            {
                ProcessKafkaMessage(stoppingToken);
                Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }

            _consumer.Close();
        }

        private FlightInfoRequest BuildFlightInfoRequest(Dictionary<string, string> dictionary){
            int flightId = Int32.Parse(dictionary["FlightId"]);
            string flightNumber = dictionary["FlightNumber"];
            string airline = dictionary["Airline"];
            string arrivalAirport = dictionary["ArrivalAirport"];
            string terminal = dictionary["Terminal"];
            string flightStatus = dictionary["FlightStatus"];

            return new FlightInfoRequest{ 
                FlightId = flightId, 
                FlightNumber = flightNumber, 
                Airline = airline, 
                ArrivalAirport = arrivalAirport, 
                Terminal = terminal,
                FlightStatus = flightStatus
            };
        }

        public void ProcessKafkaMessage(CancellationToken stoppingToken)
        {
            try
            {
                using var scope = _service.CreateScope();
                using var context = scope.ServiceProvider.GetRequiredService<FlightInfoDbContext>();

                var consumeResult = _consumer.Consume(stoppingToken);
                var message = consumeResult.Message.Value;
                var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(message);

                _logger.LogInformation($"Received flight info: {message}");
                
                if (values != null){
                    context.Database.EnsureCreatedAsync(stoppingToken);

                    var request = BuildFlightInfoRequest(values);
                    context.Add<FlightInfoRequest>(request);

                    context.SaveChangesAsync(stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing Kafka message: {ex.Message}");
            }
        }
    }
    
}