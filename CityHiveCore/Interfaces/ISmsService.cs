using CityHiveInfrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityHiveCore.Interfaces
{
    public interface ISmsService
    {
        public Task SendSmsAsync(SmsMessageModel message);
        public Task GetMessagesHistory();
    }
}
