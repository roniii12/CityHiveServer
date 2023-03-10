using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;

namespace CityHiveInfrastructure.Messaging
{
    public interface IQueueRegistrar
    {

        void RegisterQueue(IReceiveConfigurator busCfg, IBusRegistrationContext provider);
        void RegisterConsumers(IServiceCollectionBusConfigurator serviceCollectionConfigurator);
    }
    public class QueuesName
    {
        public static string SMS = "SMS";
    }
}
