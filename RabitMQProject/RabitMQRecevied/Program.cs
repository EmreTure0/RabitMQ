using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabitMQRecevied
{
    class Program
    {
        public class Log
        {
            public int LogID { get; set; }
            public string Message { get; set; }
        }

        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (IConnection connection = factory.CreateConnection())
            using (IModel channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "RabitMQTest",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    var person = JsonConvert.DeserializeObject<List<Log>>(message);
                    Console.Write(message);
                };
                channel.BasicConsume(queue: "RabitMQTest",
                                     autoAck: true,
                                     consumer: consumer);


                Console.ReadLine();
            }
        }
    }
}
