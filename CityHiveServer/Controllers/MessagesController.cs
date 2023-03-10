using CityHiveInfrastructure.Exceptions;
using CityHiveInfrastructure.Logger;
using CityHiveInfrastructure.Messaging;
using CityHiveInfrastructure.Models;
using CityHiveTwilioCore.Interfaces;
using MassTransit;
using MassTransit.RabbitMqTransport;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityHiveServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : _baseController
    {
        private readonly IAppLogger<MessagesController> _logger;
        private readonly IBus _publisher;
        private readonly ITwilioService _twilioService;
        public MessagesController(
            IAppLogger<MessagesController> logger,
            IBus publisher,
            ITwilioService twilioService
            )
        {
            //_smsService = smsService;
            _logger = logger;
            _publisher = publisher;
            _twilioService = twilioService;
        }
        [HttpPut("SendSMS")]
        public async Task<ActionResult> SendSMS([FromBody] SmsMessageModel smsMessage)
        {
            try
            {
                var host = _publisher.GetRabbitMqHostTopology().HostAddress.AbsoluteUri;
                //var endPoint = await _publisher.GetSendEndpoint(new Uri(host + QueuesName.SMS)).ConfigureAwait(false);
                await _publisher.Publish(smsMessage).ConfigureAwait(false);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Error(new ManagedException(ex, "Failed to SendSMS", AppModule.MESSAGE, AppLayer.WEB_API));
                return ReturnException(ex);
            }
        }

        [HttpGet("All")]
        public async Task<ActionResult> GetAllMessages()
        {
            try
            {
                var messages = await _twilioService.GetAllHistoryMessageAsync().ConfigureAwait(false);
                return Ok(messages);
            }
            catch (Exception ex)
            {
                _logger.Error(new ManagedException(ex, "Failed to GetAllHistoryMessages", AppModule.MESSAGE, AppLayer.WEB_API));
                return ReturnException(ex);
            }
        }
    }
}
