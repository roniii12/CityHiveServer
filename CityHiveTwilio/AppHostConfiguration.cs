using CityHiveInfrastructure.BackgroundTasks;
using CityHiveInfrastructure.Configuration;
using CityHiveInfrastructure.Extensions;
using CityHiveInfrastructure.Logger;
using CityHiveInfrastructure.Messaging;
using CityHiveTwilio.Messaging;
using CityHiveTwilio.Messaging.Consumers;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration.MultiBus;

namespace CityHiveTwilio
{
    public class AppHostConfiguration
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(typeof(AppGeneralLogger<>), typeof(AppGeneralLogger<>));
            services.AddScoped(typeof(IAppLogger<>), typeof(AppLogger<>));
            services.AddSingleton<ConfigurationService>();
            CityHiveTwilioCore.ServicesConfiguration.ConfigureServices(services);

            addMassTransit(services, configuration);
        }

        private static void addMassTransit(IServiceCollection services, IConfiguration configuration)
        {
            RabbitMQConfigModel rabbitMqConfig = configuration.GetRabbitMqSection();
            services.AddSingleton<ReceiveObserver>();
            services.AddMassTransit(x =>
            {
                var queue = new QueueRegistrar();
                queue.RegisterConsumers(x);
                x.UsingRabbitMq((cntxt, cfg) =>
                {
                    cfg.Host(new Uri(rabbitMqConfig.Url), c =>
                    {
                        c.Username(rabbitMqConfig.Username);
                        c.Password(rabbitMqConfig.Password);
                        c.UseSsl(c =>
                        {
                            c.UseCertificateAsAuthenticationIdentity = false;
                        });
                    });
                    queue.RegisterQueue(cfg, cntxt);
                });
            });
            services.AddHostedService<MassTransitHostedService>();
        }
    }
}
