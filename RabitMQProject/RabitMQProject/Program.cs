using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabitMQProject
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
            List<Log> pr = new List<Log>();
            for (int i = 0; i < 88000; i++)
            {
                pr.Add(new Log
                {
                    LogID = i,
                    Message = "RabitMQ Test"
                });
            }
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (IConnection connection = factory.CreateConnection())
            using (IModel channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "RabitMQTest",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                string message = JsonConvert.SerializeObject(pr);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: "RabitMQTest",
                                     basicProperties: null,
                                     body: body);
            }

            Console.WriteLine("Loglar Gönderildi");
            Console.ReadLine();
        }
    }
}
