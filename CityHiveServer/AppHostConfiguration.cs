using CityHiveInfrastructure.Configuration;
using CityHiveInfrastructure.Extensions;
using CityHiveInfrastructure.Logger;
using MassTransit;

namespace CityHiveServer
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
            services.AddMassTransit(x =>
            {
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
                });
            });
        }
    }
}
