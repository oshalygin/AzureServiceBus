using System;
using System.Threading;
using ASB.Constants;
using ASB.Models;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;

namespace ASB.ReceiverConsole
{
    class ReceiverApplication
    {
        private static QueueClient _client;
        static void Main(string[] args)
        {
            Console.WriteLine("Receiver Aplication - Press Enter");
            Console.ReadLine();
            //SimplePizzaReceiverLoop();
            //ProcessOrderMessages();

            ReceiveAndProcessOrderWithOnMessage(3);
            Console.WriteLine("Ending Receiver...Press any key");
            Console.ReadLine();
        }

        private static void ProcessOrderMessages()
        {
            var client = QueueClient.CreateFromConnectionString(Mother.ConnectionString, Mother.QueuePath);
            while (true)
            {
                var orderMessage = client.Peek();
                if (orderMessage != null)
                {
                    Console.WriteLine("Received Message.");
                    if (orderMessage.ContentType == "application/json")
                    {
                        var content = orderMessage.GetBody<string>();
                        Console.WriteLine($"Content: {content}");
                        Console.WriteLine($"Type: {orderMessage.Label}");
                        Console.WriteLine();
                        if (orderMessage.Label == "ASB.Models.PizzaOrder")
                        {
                            var order = JsonConvert.DeserializeObject<PizzaOrder>(content);
                            Console.WriteLine("Order Details v2:");
                            Console.WriteLine($"\t {order.CustomerName}");
                            Console.WriteLine($"\t {order.Type}");
                            Console.WriteLine($"\t {order.Size}");
                        }
                    }
                }
            }
        }

        private static void SimplePizzaReceiverLoop()
        {
            Console.WriteLine("Receiving...");

            _client = QueueClient.CreateFromConnectionString(Mother.ConnectionString, Mother.QueuePath);

            while (true)
            {
                var message = _client.Receive(TimeSpan.FromSeconds(5));
                if (message != null)
                {
                    try
                    {
                        var order = message.GetBody<PizzaOrder>();
                        CookPizza(order);
                        message.Complete();
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception.Message);
                        message.Abandon();
                    }
                }
                else
                {
                    Console.WriteLine("Nothing in the queue...");
                }
            }

        }

        private static void ReceiverLoop()
        {
            Console.WriteLine("Receiving...");

            _client = QueueClient.CreateFromConnectionString(Mother.ConnectionString, Mother.QueuePath);

            while (true)
            {
                var message = _client.Receive(TimeSpan.FromSeconds(5));
                if (message != null)
                {
                    try
                    {
                        var order = message.GetBody<string>();
                        Console.WriteLine($"Received: {order}");
                        message.Complete();
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception.Message);
                        message.Abandon();
                    }
                }
                else
                {
                    Console.WriteLine("Nothing in the queue...");
                }
            }

        }

        private static void CookPizza(PizzaOrder order)
        {
            Console.WriteLine($"Cooking {order.Type} for {order.CustomerName}");
            Thread.Sleep(5000);
            Console.WriteLine($"Done cooking {order.Type}...");
        }

        private static void ReceiveAndProcessOrderWithOnMessage(int threads)
        {
            _client = QueueClient.CreateFromConnectionString(Mother.ConnectionString, Mother.QueuePath);
            var options = new OnMessageOptions()
            {
                AutoComplete = true,
                MaxConcurrentCalls = threads,
                AutoRenewTimeout = TimeSpan.FromSeconds(30)
            };

            _client.OnMessage(message =>
            {
                var order = message.GetBody<PizzaOrder>();
                CookPizza(order);
                message.Complete();
            }, options);

            Console.WriteLine("Procesing order");
            Console.ReadLine();
            StopReceiving();
        }

        static void ReceiveAndProcessCharacters(int threads)
        {
            _client = QueueClient.CreateFromConnectionString(Mother.ConnectionString, Mother.QueuePath);

            var options = new OnMessageOptions()
            {
                AutoComplete = false,
                MaxConcurrentCalls = threads
            };

            _client.OnMessage(message =>
            {
                Console.Write(message.Label);
                message.Complete();
            }, options);
        }

        private static void StopReceiving()
        {
            _client.Close();
        }
    }
}