using CityHiveInfrastructure.Configuration;
using CityHiveInfrastructure.Models;
using CityHiveTwilioCore.Interfaces;
using CityHiveTwilioCore.Models;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace CityHiveTwilioCore.Services
{
    public class TwilioService : ITwilioService
    {
        private readonly ConfigurationService _configService;
        public TwilioService(ConfigurationService configService)
        {
            _configService = configService;
            initTwilio();
        }
        private void initTwilio()
        {
            var twilioConfig = _configService.GetTwilioConfig();
            TwilioClient.Init(twilioConfig.AccountSID, twilioConfig.Token);
        }
        public async Task<IEnumerable<MessageItemModel>> GetAllHistoryMessageAsync()
        {
            var messages = await MessageResource.ReadAsync().ConfigureAwait(false);
            return messages.Select(message => new MessageItemModel
            {
                Body = message.Body,
                Date = message.DateSent,
                To = message.To
            });
        }

        public async Task SendSmsAsync(SmsMessageModel smsMessage)
        {
            var twilioConfig = _configService.GetTwilioConfig();
            await MessageResource.CreateAsync(body: smsMessage.Message, to: new Twilio.Types.PhoneNumber(smsMessage.Phone), messagingServiceSid: twilioConfig.ServiceSID).ConfigureAwait(false);
        }
    }
}
