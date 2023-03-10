using CityHiveCore.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityHiveCore.Services
{
    public class RabitMQProducer : IMessageProducer
    {
        public RabitMQProducer()
        {

        }
        public void SendSmsMessage<T>(T message)
        {
            //Here we specify the Rabbit MQ Server. we use rabbitmq docker image and use it
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqps://b-48da2182-1588-44e2-99b3-4848c2c8361b.mq.eu-west-1.amazonaws.com:5671"),
                UserName = "admin",
                Password = "zxcvasdfqwer"
            };
            //Create the RabbitMQ connection using connection factory details as i mentioned above
            var connection = factory.CreateConnection();
            //Here we create channel with session and model
            using var channel = connection.CreateModel();
            //declare the queue after mentioning name and a few property related to that
            //channel.QueueDeclare("SMS", exclusive: false);
            //Serialize the message
            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);
            //put the data on to the product queue
            channel.BasicPublish(exchange: "", routingKey: "SMS", body: body);
        }
    }
}
