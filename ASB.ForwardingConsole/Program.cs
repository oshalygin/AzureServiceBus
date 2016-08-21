using System;
using ASB.Constants;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace ASB.ForwardingConsole
{
    class ForwardingApplication
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Forwarder Console");
            Console.WriteLine();
            CreateQueue();
            ForwardMessages();

        }


        static void ForwardMessages()
        {
            // Create queue clients
            var inboundQueueClient = QueueClient.CreateFromConnectionString(Mother.ConnectionString, Mother.QueuePath);
            var outboundQueueClient = QueueClient.CreateFromConnectionString(Mother.ConnectionString, Mother.ForwardingPath);

            Console.WriteLine("Forwarding messages, hit enter to exit");

            inboundQueueClient.OnMessage(message =>
            {
                // Without message cloning
                outboundQueueClient.Send(message);

                // Clone the message
                var outboundMessage = message.Clone();

                outboundQueueClient.Send(outboundMessage);

                Console.WriteLine("Forwarded message: " + message.Label);
            });


            Console.ReadLine();

            // Close the clients to free up connections.
            inboundQueueClient.Close();
            outboundQueueClient.Close();
        }

        static void CreateQueue()
        {
            var manager = NamespaceManager.CreateFromConnectionString(Mother.ConnectionString);
            if (!manager.QueueExists(Mother.ForwardingPath))
            {
                Console.Write("Creating queue: " + Mother.ForwardingPath + "...");
                manager.CreateQueue(Mother.ForwardingPath);
                Console.WriteLine("Done!");
            }
        }
    }
   
}
