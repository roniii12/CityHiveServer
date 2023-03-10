using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityHiveInfrastructure.Configuration
{
    public class ConfigurationService
    {
        private readonly IConfiguration _config;
        public ConfigurationService(IConfiguration config)
        {
            _config = config;
        }
        public TwilioConfig GetTwilioConfig()
        {
            return _config.GetSection("Twilio").Get<TwilioConfig>()!;
        }
    }
}
