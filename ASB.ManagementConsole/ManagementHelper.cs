using System;
using ASB.Constants;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace ASB.ManagementConsole
{
    public class ManagementHelper
    {
        private NamespaceManager _namespaceManager;

        public ManagementHelper()
        {
            _namespaceManager = NamespaceManager.CreateFromConnectionString(Mother.ConnectionString);
            Console.WriteLine($"Service Bus Address: {_namespaceManager.Address}");
        }

        public void CreateQueue(string queuePath)
        {
            Console.WriteLine($"Creating {queuePath} queue....");
            _namespaceManager.CreateQueue(new QueueDescription(queuePath));
            Console.WriteLine($"Done creating the queue {queuePath}...");
        }

        public void DeleteQueue(string queuePath)
        {
            Console.WriteLine($"Deleting {queuePath} queue....");
            _namespaceManager.DeleteQueue(queuePath);
            Console.WriteLine($"Done deleting the queue {queuePath}...");
        }

        public void ListQueues()
        {
            Console.WriteLine($"Listing the queues");
            var queues = _namespaceManager.GetQueues();
            foreach (var queue in queues)
            {
                Console.WriteLine($"\t{queue.Path}");

            }
        }

        public void GetQueue(string queuePath)
        {
            var queue = _namespaceManager.GetQueue(queuePath);
            Console.WriteLine($"Queue MessageCount: {queue.MessageCount}");
            Console.WriteLine($"Queue SizeInBytes: {queue.SizeInBytes}");
            Console.WriteLine($"Queue RequireSession: {queue.RequiresSession}");
            Console.WriteLine($"Queue IsReadOnly: {queue.IsReadOnly}");
            Console.WriteLine($"Queue MaxDeliveryCount: {queue.MaxDeliveryCount}");
        }

        public void CreateTopic(string topicName)
        {
            Console.WriteLine($"Creating {topicName} topic....");
            _namespaceManager.CreateTopic(new TopicDescription(topicName));
            Console.WriteLine($"Done creating the topic {topicName}...");
        }

        public void DeletingTopic(string topicName)
        {
            Console.WriteLine($"Deleting {topicName} topic....");
            _namespaceManager.DeleteTopic(topicName);
            Console.WriteLine($"Done deleting the topic {topicName}...");
        }

        public void CreateSubscription(string topicName, string subscriptionName)
        {
            Console.WriteLine($"Creating {subscriptionName} subscription in the {topicName} topic....");
            _namespaceManager.CreateSubscription(new SubscriptionDescription(topicName, subscriptionName));
            Console.WriteLine($"Done creating the subscription {subscriptionName}...");
        }

        public void ListTopicsAndSubscriptions()
        {
            Console.WriteLine("Listing topics and subscriptions");
            var topics = _namespaceManager.GetTopics();
            foreach (var topic in topics)
            {
                Console.WriteLine($"\tTopic: {topic.Path}");
                var subscriptions = _namespaceManager.GetSubscriptions(topic.Path);
                foreach (var subscription in subscriptions)
                {
                    Console.WriteLine($"\t\tSubscription: {subscription.Name}");
                }
            }

            Console.WriteLine("Done Listing the topics and subscriptions");
        }


    }
}