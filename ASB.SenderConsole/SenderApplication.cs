using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ASB.Constants;
using ASB.Models;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace ASB.SenderConsole
{
    public class SenderApplication
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Sender Application - Hit enter");
            Console.ReadLine();
            //SendJsonMessage(new PizzaOrder
            //{
            //    CustomerName = "Carlota Turcios",
            //    Size = "Large",
            //    Type = "Hawaiian"
            //});
            SendPizzaOrderBatch();
            Console.WriteLine();
            Console.WriteLine("Sender Application - Complete");
            Console.ReadLine();
        }

        static void SendTextString(string text, bool sendSync)
        {
            var client = QueueClient.CreateFromConnectionString(Mother.ConnectionString, Mother.QueuePath);

            Console.WriteLine("Sending....");

            var taskList = new List<Task>();


            foreach (var letter in text)
            {
                var message = new BrokeredMessage {Label = letter.ToString()};

                if (sendSync)
                {
                    client.Send(message);
                    Console.Write(message.Label);
                }
                else
                {
                    taskList.Add(
                        client
                            .SendAsync(message)
                            .ContinueWith(task => Console.WriteLine($"Sent: {message.Label}")));
                }
            }
            if (!sendSync)
            {
                Console.WriteLine("Waiting...");
                Task.WaitAll(taskList.ToArray());
                Console.WriteLine("Completed Async");
            }

            client.Close();
        }

        static void SlideCode()
        {
            var client = TopicClient.CreateFromConnectionString("");
            var message = new BrokeredMessage();
            if (message.Size > 250*1024)
            {
                throw new ArgumentException("Message is too large");
            }

            client.Send(message);
            client.Close();

            var sendTask = client.SendAsync(message).ContinueWith(task => Console.WriteLine($"Sent {message.Label}"));

            client.Close();
        }

        static void SendControlMessage()
        {
            var message = new BrokeredMessage()
            {
                Label = "Control"
            };

            message.Properties.Add("SystemId", 1462);
            message.Properties.Add("Command", "Pending Restart");
            message.Properties.Add("ActionTime", DateTime.UtcNow.AddHours(2));

            var client = QueueClient.CreateFromConnectionString(Mother.ConnectionString, Mother.QueuePath);
            Console.WriteLine("Sending Control Message");
            client.Send(message);
            Console.WriteLine("Done!");

            Console.WriteLine("Send again?");
            var response = Console.ReadLine();
            if (response != null && response.ToLower().StartsWith("y"))
            {
                Console.WriteLine("Sending another control message");
                message = message.Clone();
                client.Send(message);
                Console.WriteLine("Done sending a clone!");
            }

            client.Close();
        }

        static void SendPizzaOrder()
        {
            var order = new PizzaOrder()
            {
                CustomerName = "Oleg Shalygin",
                Type = "Hawaiian",
                Size = "Large"
            };

            var message = new BrokeredMessage(order) {Label = "PizzaOrder"};

            Console.WriteLine($"Message size: {message.Size}");

            var client = QueueClient.CreateFromConnectionString(Mother.ConnectionString, Mother.QueuePath);

            Console.WriteLine("Sending Order...");
            client.Send(message);
            Console.WriteLine("Done sending order!");
            client.Close();

            Console.WriteLine($"Message size: {message.Size}");
        }

        static void SendPizzaOrderBatch()
        {
            var names = new[] {"Allan", "Jennifer", "James"};
            var pizzas = new[] {"Hawaiian", "Vegitarian", "Capricciosa", "naPolian"};

            var client = QueueClient.CreateFromConnectionString(Mother.ConnectionString, Mother.QueuePath);

            var taskList = new List<Task>();
            foreach (var pizza in pizzas)
            {
                foreach (var name in names)
                {
                    var order = new PizzaOrder()
                    {
                        CustomerName = name,
                        Type = pizza,
                        Size = "Large"
                    };
                    var message = new BrokeredMessage(order);
                    taskList.Add(client.SendAsync(message));
                }
            }

            Console.WriteLine("Sending Batch!");
            Task.WaitAll(taskList.ToArray());
            Console.WriteLine("Sent batch!");
        }

        private static void CreateQueue(string queueName)
        {
            var namespaceManager = NamespaceManager.CreateFromConnectionString(Mother.ConnectionString);
            var createdQueue = namespaceManager.CreateQueue(queueName);
            Console.WriteLine($"Created {createdQueue.Path}");
        }

        private static void SendJsonMessage(object content)
        {
            var settings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = new List<JsonConverter> {new StringEnumConverter()}
            };

            var json = JsonConvert.SerializeObject(content, settings);

            Console.WriteLine($"Json Content: {json}");
            var message = new BrokeredMessage(json)
            {
                ContentType = @"application/json",
                Label = content.GetType().ToString()
            };
            Console.WriteLine("Sending message...");
            var client = QueueClient.CreateFromConnectionString(Mother.ConnectionString, Mother.QueuePath);
            client.Send(message);
            Console.WriteLine("Sent Message!");
            client.Close();
        }
    }
}