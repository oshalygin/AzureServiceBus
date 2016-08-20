using System;
using Microsoft.ServiceBus.Messaging;

namespace ASB.SenderConsole
{
    public class Program
    {
      
        static void Main(string[] args)
        {
            var client = QueueClient.CreateFromConnectionString(Constants.Mother.ConnectionString, Constants.Mother.QueuePath);
            for (var i = 1; i <= 10; i++)
            {
                var message = new BrokeredMessage($"Message #{i}");
                client.Send(message);
                Console.WriteLine($"Sent: {message.GetBody<string>()}");
            }
            client.Close();
            Console.ReadLine();
        }
    }
}