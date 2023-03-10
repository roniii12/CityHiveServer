using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityHiveInfrastructure.Configuration
{
    public class RabbitMQConfigModel
    {
        public string Url { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class TwilioConfig
    {
        public string AccountSID { get; set; }
        public string Token { get; set; }
        public string ServiceSID { get; set; }
        public string PhoneNumber { get; set; }
    }
}
