using CityHiveInfrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityHiveInfrastructure.Extensions
{
    public static class ConfigurationExtensions
    {
        public static RabbitMQConfigModel GetRabbitMqSection(this IConfiguration Configuration)
        {
            return Configuration.GetSection("RabbitMQ").Get<RabbitMQConfigModel>()!;
        }
    }
}
