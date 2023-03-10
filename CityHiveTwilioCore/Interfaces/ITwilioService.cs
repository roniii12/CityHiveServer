using CityHiveInfrastructure.Models;
using CityHiveTwilioCore.Models;

namespace CityHiveTwilioCore.Interfaces
{
    public interface ITwilioService
    {
        public Task SendSmsAsync(SmsMessageModel smsMessage);
        public Task<IEnumerable<MessageItemModel>> GetAllHistoryMessageAsync();
    }
}
