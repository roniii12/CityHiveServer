using CityHiveTwilioCore.Interfaces;
using CityHiveTwilioCore.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CityHiveTwilioCore
{
    public class ServicesConfiguration
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ITwilioService, TwilioService>();
        }
    }
}
