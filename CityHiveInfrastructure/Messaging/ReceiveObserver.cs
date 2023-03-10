using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CityHiveInfrastructure.Exceptions;
using CityHiveInfrastructure.Logger;
using MassTransit;
using Newtonsoft.Json;
//using Newtonsoft.Json;

namespace CityHiveInfrastructure.Messaging
{
    public class ReceiveObserver : IReceiveObserver, IConsumeObserver, ISendObserver, IPublishObserver
    {
        // private readonly Telemetry<MessagingTelemetry> _telemetry = null;
        private readonly AppGeneralLogger<ReceiveObserver> _logger;
        public ReceiveObserver(AppGeneralLogger<ReceiveObserver> logger)//, Telemetry<MessagingTelemetry> telemetry)
        {
            _logger = logger;
        }


        ~ReceiveObserver()  // finalizer
        {

        }

        private void logEvent(string eventName, Dictionary<string, string> properties)
        {
            _logger.Info($"{DateTime.Now.ToLongTimeString()} - {eventName}");
        }

        private void logError(string eventName, Exception ex, Dictionary<string, string> properties = null)
        {
            var mngException = new ManagedException(ex, $"Failed to {eventName} ", AppModule.GENERAL_HANDLER, AppLayer.BUSCONSUMERS);
            if (properties != null)
                foreach (var prop in properties)
                    mngException.Data[prop.Key] = prop.Value;

            _logger.Error(mngException);
        }

        private Dictionary<string, string> getProps(ReceiveContext context, Exception exception = null)
        {
            var dic = context.TransportHeaders?.GetAll()?.ToDictionary(k => k.Key, v => v.Value.ToString());
            if (dic == null)
                dic = new Dictionary<string, string>();




            if (exception != null)
            {
                dic.Add("exception.Message", exception.Message);
                dic.Add("exception.StackTrace", exception.StackTrace);
                if (exception.InnerException != null)
                    dic.Add("InnerException.Message", exception.InnerException.Message);
            }

            return dic;
        }
        private Dictionary<string, string> getProps<T>(ConsumeContext<T> context, Exception exception = null) where T : class
        {
            var dic = context.Headers?.GetAll()?.ToDictionary(k => k.Key, v => v.Value.ToString());
            if (dic == null)
                dic = new Dictionary<string, string>();


            var message = context as MassTransit.Context.MessageConsumeContext<T>;
            if (message != null && message.Message != null)
            {
                dic.Add("FullMessage", JsonConvert.SerializeObject(message.Message));
            }


            if (exception != null)
            {
                dic.Add("exception.Message", exception.Message);
                dic.Add("exception.StackTrace", exception.StackTrace);
                if (exception.InnerException != null)
                    dic.Add("InnerException.Message", exception.InnerException.Message);
            }


            dic.Add("MessageId", context.MessageId?.ToString());
            dic.Add("CorrelationId", context.CorrelationId?.ToString());
            dic.Add("DestinationAddress", context.DestinationAddress?.AbsoluteUri);
            dic.Add("SourceAddress", context.SourceAddress?.AbsoluteUri);
            dic.Add("FaultAddress", context.FaultAddress?.AbsoluteUri);
            dic.Add("ResponseAddress", context.ResponseAddress?.AbsoluteUri);
            dic.Add("ConversationId", context.ConversationId?.ToString());
            dic.Add("InitiatorId", context.InitiatorId?.ToString());
            dic.Add("RequestId", context.RequestId?.ToString());

            return dic;
        }


        private Dictionary<string, string> getProps<T>(SendContext<T> context, Exception exception = null) where T : class
        {
            var dic = context.Headers?.GetAll()?.ToDictionary(k => k.Key, v => v.Value.ToString());
            if (dic == null)
                dic = new Dictionary<string, string>();


            var message = context.Message;
            if (context.Message != null)
            {
                dic.Add("FullMessage", JsonConvert.SerializeObject(context.Message));
            }

            if (exception != null)
            {
                dic.Add("exception.Message", exception.Message);
                dic.Add("exception.StackTrace", exception.StackTrace);
                if (exception.InnerException != null)
                    dic.Add("InnerException.Message", exception.InnerException.Message);
            }

            dic.Add("MessageId", context.MessageId?.ToString());
            dic.Add("CorrelationId", context.CorrelationId?.ToString());
            dic.Add("DestinationAddress", context.DestinationAddress?.AbsoluteUri);
            dic.Add("SourceAddress", context.SourceAddress?.AbsoluteUri);
            dic.Add("FaultAddress", context.FaultAddress?.AbsoluteUri);
            dic.Add("ResponseAddress", context.ResponseAddress?.AbsoluteUri);
            dic.Add("ConversationId", context.ConversationId?.ToString());
            dic.Add("InitiatorId", context.InitiatorId?.ToString());
            dic.Add("RequestId", context.RequestId?.ToString());

            return dic;
        }





        public Task PreReceive(ReceiveContext context)
        {
            // called immediately after the message was delivery by the transport
            //var dic = getProps(context);

            //logEvent("PreReceive", dic);
            return Task.CompletedTask;
        }

        public Task PostReceive(ReceiveContext context)
        {
            // called after the message has been received and processed
            //var dic = getProps(context);
            //logEvent("PostReceive", dic);
            return Task.CompletedTask;
        }

        public Task PostConsume<T>(ConsumeContext<T> context, TimeSpan duration, string consumerType)
            where T : class
        {


            var dic = getProps(context);
            dic.Add("Duration", duration.ToString());
            dic.Add("consumerType", consumerType);

            logEvent("PostConsume_" + typeof(T).Name, dic);

            return Task.CompletedTask;
        }

        public Task ConsumeFault<T>(ConsumeContext<T> context, TimeSpan elapsed, string consumerType, Exception exception) where T : class
        {
            // called when the message is consumed but the consumer throws an exception
            var dic = getProps(context, exception);
            dic.Add("Duration", elapsed.ToString());
            dic.Add("consumerType", consumerType);

            logEvent("ConsumeFault_" + typeof(T).Name, dic);
            logError("ConsumeFault_" + typeof(T).Name, exception, dic);

            return Task.CompletedTask;
        }

        public Task ReceiveFault(ReceiveContext context, Exception exception)
        {
            // called when an exception occurs early in the message processing, such as deserialization, etc.
            var dic = getProps(context, exception);

            logEvent("ReceiveFault", dic);
            logError("ReceiveFault", exception, dic);
            return Task.CompletedTask;
        }

        public Task PreConsume<T>(ConsumeContext<T> context) where T : class
        {
            var dic = getProps(context);


            logEvent("PreConsume_" + typeof(T).Name, dic);
            return Task.CompletedTask;
        }

        public Task PostConsume<T>(ConsumeContext<T> context) where T : class
        {
            return Task.CompletedTask;
        }

        public Task ConsumeFault<T>(ConsumeContext<T> context, Exception exception) where T : class
        {
            return Task.CompletedTask;
        }

        public Task PreSend<T>(SendContext<T> context) where T : class
        {
            return Task.CompletedTask;
        }

        public Task PostSend<T>(SendContext<T> context) where T : class
        {
            var dic = getProps(context);
            logEvent("PostSend_" + typeof(T).Name, dic);

            return Task.CompletedTask;
        }

        public Task SendFault<T>(SendContext<T> context, Exception exception) where T : class
        {
            var dic = getProps(context, exception);

            logEvent("SendFault_" + typeof(T).Name, dic);
            logError("SendFault_" + typeof(T).Name, exception, dic);

            return Task.CompletedTask;
        }

        public Task PrePublish<T>(PublishContext<T> context) where T : class
        {
            return Task.CompletedTask;
        }

        public Task PostPublish<T>(PublishContext<T> context) where T : class
        {
            var dic = getProps(context);

            logEvent("PostPublish_" + typeof(T).Name, dic);

            return Task.CompletedTask;
        }

        public Task PublishFault<T>(PublishContext<T> context, Exception exception) where T : class
        {

            var dic = getProps(context, exception);

            logEvent("PublishFault_" + typeof(T).Name, dic);
            logError("PublishFault_" + typeof(T).Name, exception, dic);
            return Task.CompletedTask;
        }
    }


}
