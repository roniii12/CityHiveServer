using CityHiveInfrastructure.Messaging;
using CityHiveTwilio.Messaging.Consumers;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;

namespace CityHiveTwilio.Messaging
{
    public class QueueRegistrar : IQueueRegistrar
    {
        public void RegisterQueue(IReceiveConfigurator busCfg, IBusRegistrationContext provider)
        {

            busCfg.ReceiveEndpoint(QueuesName.SMS, e =>
            {
                e.Consumer<SmsMessageConsumer>(provider);
            });
        }

        public void RegisterConsumers(IServiceCollectionBusConfigurator serviceCollectionConfigurator)
        {
            serviceCollectionConfigurator.AddConsumer<SmsMessageConsumer>();
        }
    }
}
