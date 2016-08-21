using System;
using System.Threading;
using ASB.Constants;
using DataContracts;
using Microsoft.ServiceBus.Messaging;

namespace ASB.ReceiverConsole
{
    class ReceiverApplication
    {
        private static QueueClient _client;
        static void Main(string[] args)
        {
            Console.WriteLine("Receiver Console - Press Enter");
            Console.ReadLine();
            ReceiveAndProcessCharacters(3);
            //SimplePizzaReceiverLoop();
            Console.WriteLine("Ending Receiver...Press any key");
            Console.ReadLine();
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