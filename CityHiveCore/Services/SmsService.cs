using CityHiveCore.Interfaces;
using CityHiveInfrastructure.Models;

namespace CityHiveCore.Services
{
    public class SmsService : ISmsService
    {
        private readonly IMessageProducer _messageProducer;
        public SmsService(IMessageProducer messageProducer)
        {
            _messageProducer = messageProducer;
        }

        public Task GetMessagesHistory()
        {
            throw new NotImplementedException();
        }

        public async Task SendSmsAsync(SmsMessageModel message)
        {
            _messageProducer.SendSmsMessage(message);
        }
    }
}
