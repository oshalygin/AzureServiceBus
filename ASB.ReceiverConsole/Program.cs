using System;
using Microsoft.ServiceBus.Messaging;

namespace ASB.ReceiverConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = QueueClient.CreateFromConnectionString(Constants.Mother.ConnectionString,
                Constants.Mother.QueuePath);
            client.OnMessage(ProcessMessage);
            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
            client.Close();
        }

        private static void ProcessMessage(BrokeredMessage message)
        {
            var messageBody = message.GetBody<string>();
            var messageId = message.MessageId;
            Console.WriteLine($"Received: {messageId} - {messageBody}");
        }
    }
}