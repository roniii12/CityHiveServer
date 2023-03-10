using CityHiveInfrastructure.Exceptions;
using CityHiveInfrastructure.Logger;
using CityHiveInfrastructure.Messaging;
using CityHiveInfrastructure.Models;
using CityHiveTwilioCore.Interfaces;
using MassTransit;
using MassTransit.RabbitMqTransport.Integration;

namespace CityHiveTwilio.Messaging.Consumers
{
    public class SmsMessageConsumer : IConsumer<SmsMessageModel>
    {
        private readonly IAppLogger<SmsMessageConsumer> _logger;
        private readonly ITwilioService _twilioService;
        public SmsMessageConsumer(IAppLogger<SmsMessageConsumer> logger, ITwilioService twilioService)
        {
            _logger = logger;
            _twilioService = twilioService;
        }
        public async System.Threading.Tasks.Task Consume(ConsumeContext<SmsMessageModel> context)
        {

            try
            {
                await _twilioService.SendSmsAsync(context.Message).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.Error(new ManagedException(ex, "Failed to SendSMSViaTwilio", AppModule.MESSAGE, AppLayer.BUSCONSUMERS));
            }
        }
    }
}
