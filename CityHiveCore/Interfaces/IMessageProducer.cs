using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityHiveCore.Interfaces
{
    public interface IMessageProducer
    {
        public void SendSmsMessage<T>(T message);
    }
}
