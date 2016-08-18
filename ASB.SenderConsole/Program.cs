using System;
using Microsoft.ServiceBus.Messaging;

namespace ASB.SenderConsole
{
    public class Program
    {
        public const string ConnectionString = @"Endpoint=sb://asboleg.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=4ac/HxC9hMAeDE3Y1MxmPuByCYM9Z+S8WkRKhnWZINM=";
        public const string QueuePath = "firstqueue";

        static void Main(string[] args)
        {
            var queueClient = QueueClient.CreateFromConnectionString(ConnectionString, QueuePath);
            for (var i = 0; i < 10; i++)
            {
                var message = new BrokeredMessage($"Message #{i}");
                queueClient.Send(message);
                Console.WriteLine($"Sent - {message.GetBody<string>()}");
            }
            queueClient.Close();
        }
    }
}