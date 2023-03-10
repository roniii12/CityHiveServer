using CityHiveCore.Interfaces;
using CityHiveCore.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CityHiveCore
{
    public class ServicesConfiguration
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IMessageProducer, RabitMQProducer>();
            services.AddScoped<ISmsService, SmsService>();
        }
    }
}
