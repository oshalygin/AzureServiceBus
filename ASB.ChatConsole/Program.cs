using System;
using ASB.Constants;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace ASB.ChatConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter your name: ");
            var userName = Console.ReadLine();

            var manager = NamespaceManager.CreateFromConnectionString(Mother.ConnectionString);
            if (!manager.TopicExists(Mother.TopicPath))
            {
                manager.CreateTopic(Mother.TopicPath);
            }

            manager.CreateSubscription(new SubscriptionDescription(Mother.TopicPath, userName)
            {
                AutoDeleteOnIdle = TimeSpan.FromMinutes(5)
            });

            var factory = MessagingFactory.CreateFromConnectionString(Mother.ConnectionString);
            var topicClient = factory.CreateTopicClient(Mother.TopicPath);
            var subscriptionClient = factory.CreateSubscriptionClient(Mother.TopicPath, userName);

            subscriptionClient.OnMessage(ProcessMessage);

            var helloMessage = new BrokeredMessage("Has entered the room....") {Label = userName};
            topicClient.Send(helloMessage);

            while (true)
            {
                var text = Console.ReadLine();
                if (text != null && text.ToLower() == "exit")
                {
                    break;
                }

                var chatMessage = new BrokeredMessage(text) {Label = userName};
                topicClient.Send(chatMessage);
            }

            var goodbyeMessage = new BrokeredMessage($"{userName} has left the room...") {Label = userName};
            topicClient.Send(goodbyeMessage);

            factory.Close();
        }

        private static void ProcessMessage(BrokeredMessage message)
        {
            Console.WriteLine($"{message.Label} > {message.GetBody<string>()}");
        }
    }
}